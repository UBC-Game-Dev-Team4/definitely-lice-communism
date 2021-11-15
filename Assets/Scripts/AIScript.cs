using System;
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
        SpecificDirection
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
            SetMode(mode);
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
                float delay = Random.Range(minDelayBeforeWander, maxDelayBeforeWander);
                yield return new WaitForSeconds(delay);
                float duration = Random.Range(minWanderDuration, maxWanderDuration);
                shouldBeMoving = true;
                if (transform.position.x <= wanderLockXLeft) directionIsLeft = false;
                else if (transform.position.x >= wanderLockXRight) directionIsLeft = true;
                else directionIsLeft = Random.Range(0, 2) == 1;
                yield return new WaitForSeconds(duration);
                shouldBeMoving = false;
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
            if (transform.position.x <= wanderLockXLeft && directionIsLeft)
                shouldBeMoving = false;
            else if (transform.position.x >= wanderLockXRight && !directionIsLeft)
                shouldBeMoving = false;
            if (shouldBeMoving)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);
                }
            } else if (currentSpeed > 0)
            {
                currentSpeed = Mathf.Max(0, currentSpeed - deceleration * Time.fixedDeltaTime);
            }
            body.MovePosition(body.position + new Vector2((directionIsLeft ? -1 : 1) * currentSpeed * Time.fixedDeltaTime,0));
        }
        
        /// <summary>
        /// Properly sets the mode and restarts coroutines
        /// </summary>
        /// <param name="newMode">New AIMode to set to</param>
        /// <exception cref="ArgumentOutOfRangeException">If invalid enum parameter is used</exception>
        public virtual void SetMode(AIMode newMode)
        {
            mode = newMode;
            StopAllCoroutines();
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}