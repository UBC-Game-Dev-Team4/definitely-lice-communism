﻿using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    /// <summary>
    /// Mode of NPC AI movement
    /// </summary>
    public enum AIMode
    {
        /// <summary>
        /// Is standing in place
        /// </summary>
        Stationary,
        /// <summary>
        /// Can wander, but only in some area
        /// </summary>
        WanderLocked,
        /// <summary>
        /// Can wander unrestricted
        /// </summary>
        Wander,
        /// <summary>
        /// Going in a specific direction
        /// </summary>
        SpecificDirection,
        /// <summary>
        /// Going to specific X
        /// </summary>
        SpecificX
    }
    
    /// <summary>
    /// Script attached to an NPC/AI
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class AIScript : MonoBehaviour
    {
        [Tooltip("Type of movement")]
        public AIMode mode = AIMode.Stationary;

        [Tooltip("Minimum delay before wandering")]
        public float minDelayBeforeWander = 2;
        [Tooltip("Maximum delay before wandering")]
        public float maxDelayBeforeWander = 4;
        [Tooltip("Minimum wander duration")]
        public float minWanderDuration = 2f;
        [Tooltip("Maximum wander duration")]
        public float maxWanderDuration = 4f;
        [Tooltip("Wander X left bound")]
        public float wanderLockXLeft = -1;
        [Tooltip("Wander X right bound")]
        public float wanderLockXRight = -1;
        [Tooltip("Target X position")]
        public float targetX = -1;
        [Tooltip("Target X Tolerance")]
        public float targetXTolerance = 0.5f;
        [Tooltip("Max speed")]
        public float maxSpeed = 5;
        [Tooltip("Acceleration while moving")]
        public float acceleration = 2.5f;
        [Tooltip("Deceleration while not moving")]
        public float deceleration = 7.5f;
        [Tooltip("Whether the cat should move left or right")]
        public bool directionIsLeft;
        protected bool shouldBeMoving;
        protected float currentSpeed;
        protected Rigidbody2D body;

        /// <summary>
        /// On awake, locate/initialize required components
        /// </summary>
        protected virtual void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            SetMode(mode, true);
        }

        protected virtual IEnumerator ToSpecificXCoroutine()
        {
            float currentX = transform.position.x;
            float currentGoal = targetX;
            shouldBeMoving = true;
            directionIsLeft = currentGoal < currentX;
            while (mode == AIMode.SpecificX)
            {
                bool shouldRecalculateDir = currentGoal == targetX;
                if (shouldRecalculateDir)
                {
                    currentGoal = targetX;
                    directionIsLeft = currentGoal < currentX;
                }

                currentX = transform.position.x;
                if (Math.Abs(currentGoal - currentX) <= targetXTolerance) break;
                // goal is on right; direction is left
                if (currentGoal - currentX >= 0 && directionIsLeft) break;
                // goal is on left; direction is right
                if (currentGoal - currentX <= 0 && !directionIsLeft) break;
                yield return new WaitForFixedUpdate();
            }
            
            if (mode == AIMode.SpecificX) OnReachSpecificXDestination();

            shouldBeMoving = false;
        }

        /// <summary>
        /// Coroutine for wandering within bounds.
        /// </summary>
        /// <remarks>
        /// NPC can exceed bounds if deceleration isn't infinite.
        /// </remarks>
        protected virtual IEnumerator WanderLockedCoroutine()
        {
            while (mode == AIMode.WanderLocked)
            {
                shouldBeMoving = false;
                float delay = Random.Range(minDelayBeforeWander, maxDelayBeforeWander);
                yield return new WaitForSeconds(delay);
                float duration = Random.Range(minWanderDuration, maxWanderDuration);
                shouldBeMoving = true;
                if (transform.position.x <= wanderLockXLeft) directionIsLeft = false;
                else if (transform.position.x >= wanderLockXRight) directionIsLeft = true;
                else directionIsLeft = Random.Range(0, 2) == 1;
                yield return new WaitForSeconds(duration);
            }
        }

        /// <summary>
        /// Coroutine for wandering in an unrestricted fashion
        /// </summary>
        protected virtual IEnumerator WanderCoroutine()
        {
            while (mode == AIMode.Wander)
            {
                float delay = Random.Range(minDelayBeforeWander, maxDelayBeforeWander);
                yield return new WaitForSeconds(delay);
                float duration = Random.Range(minWanderDuration, maxWanderDuration);
                shouldBeMoving = true;
                directionIsLeft = Random.Range(0, 1) == 1;
                yield return new WaitForSeconds(duration);
                shouldBeMoving = false;
            }
        }

        /// <summary>
        /// Every physics update, move the NPC
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (mode == AIMode.WanderLocked)
            {
                if (transform.position.x <= wanderLockXLeft && directionIsLeft)
                    shouldBeMoving = false;
                else if (transform.position.x >= wanderLockXRight && !directionIsLeft)
                    shouldBeMoving = false;
            }

            if (shouldBeMoving)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);
                }
            } else if (currentSpeed > 0)
            {
                if (deceleration * Time.fixedDeltaTime >= currentSpeed) currentSpeed = 0;
                else currentSpeed -= deceleration * Time.fixedDeltaTime;
            }
            if (currentSpeed != 0)
                body.MovePosition(body.position + new Vector2((directionIsLeft ? -1 : 1) * currentSpeed * Time.fixedDeltaTime,0));
        }

        /// <summary>
        /// Properly sets the mode and restarts coroutines
        /// </summary>
        /// <param name="newMode">New AIMode to set to</param>
        /// <param name="forceSet">If true, sets the mode and starts coroutines anyways if newMode is the same</param>
        /// <exception cref="ArgumentOutOfRangeException">If invalid enum parameter is used</exception>
        public virtual void SetMode(AIMode newMode, bool forceSet = false)
        {
            if (newMode == mode && !forceSet) return;
            Debug.Log("New Mode: " + newMode);
            mode = newMode;
            StopCoroutine(nameof(WanderCoroutine));
            StopCoroutine(nameof(WanderLockedCoroutine));
            StopCoroutine(nameof(ToSpecificXCoroutine));
            switch (newMode)
            {
                case AIMode.Wander:
                    StartCoroutine(nameof(WanderCoroutine));
                    break;
                case AIMode.WanderLocked:
                    StartCoroutine(nameof(WanderLockedCoroutine));
                    break;
                case AIMode.SpecificDirection:
                    shouldBeMoving = true;
                    break;
                case AIMode.Stationary:
                    shouldBeMoving = false;
                    break;
                case AIMode.SpecificX:
                    StartCoroutine(nameof(ToSpecificXCoroutine));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void OnReachSpecificXDestination()
        {
        }

        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            switch (mode)
            {
                case AIMode.SpecificX:
                {
                    Vector3 position = transform.position;
                    Gizmos.DrawWireSphere(new Vector3(targetX,position.y,position.z), targetXTolerance);
                    break;
                }
                case AIMode.SpecificDirection:
                    Gizmos.DrawRay(transform.position, new Vector3(directionIsLeft ? -1 : 1, 0, 0));
                    break;
                case AIMode.WanderLocked:
                {
                    var position = transform.position;
                    Vector3 center = new Vector3((wanderLockXRight + wanderLockXLeft)/2, position.y, position.z);
                    Vector3 size = new Vector3((wanderLockXRight - wanderLockXLeft) / 2f, 2, 2);
                    Gizmos.DrawWireCube(center,size);
                    break;
                }
            }
            Gizmos.color = Color.white;
        }
    }
}