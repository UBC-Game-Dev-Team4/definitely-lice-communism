﻿using System.Collections;
using Player;
using UnityEngine;
using Util;

namespace DefaultNamespace
{
    /// <summary>
    ///     Script attached to an interactable door
    /// </summary>
    public class DoorScript : Interactable
    {
        [Tooltip("Position to teleport player on interaction")]
        public Vector3 positionOnInteract;

        [Tooltip("Area to teleport to/camera state to set")]
        public AreaScript areaToTeleportTo;

        [Tooltip("Delay after door open animation before player disappears")]
        public float delayBeforeEnter = 1;
        
        [Tooltip("Delay after player disappearing before door closes")]
        public float delayJustAfterEnter = 0.4f;
        [Tooltip("Delay after door close animation starts before camera is set correctly")]
        public float delayAfterDoorClose = 0.4f;

        [Tooltip("Area of Door")]
        public AreaScript currentArea;
        
        private Animator _animator;
        private static readonly int OpenTrigger = Animator.StringToHash("OpenTrigger");
        private static readonly int CloseTrigger = Animator.StringToHash("CloseTrigger");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_animator == null) Debug.Log("Animator is null!!!!");
        }

        /// <summary>
        ///     Draw a wireframe sphere around the position to teleport to
        /// </summary>
        /// <inheritdoc cref="MonoBehaviour" />
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(positionOnInteract, 0.5f);
        }

        /// <inheritdoc cref="Interactable" />
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            if (!(src is PlayerScript player)) return;
            StartCoroutine(nameof(DoorInteractCoroutine), player);
        }

        /// <summary>
        /// Coroutine that runs when a player interacts with the door
        /// </summary>
        /// <param name="player">Player that interacted with this door script</param>
        public IEnumerator DoorInteractCoroutine(PlayerScript player)
        {
            player.StopMoving();
            player.movementEnabled = false;
            player.interactionEnabled = false;
            bool skipPlay = false;
            if (_animator != null)
                _animator.SetTrigger(OpenTrigger);
            if (currentArea != null && areaToTeleportTo != null)
            {
                if (areaToTeleportTo.HasBackgroundMusic() && AreaScript.CurrentlyActiveBackgroundMusic != areaToTeleportTo.backgroundMusic &&
                    (AreaScript.CurrentlyActiveBackgroundMusic == null || areaToTeleportTo.backgroundMusic == null || AreaScript.CurrentlyActiveBackgroundMusic.clip != areaToTeleportTo.backgroundMusic.clip))
                    currentArea.FadeBackgroundMusic(delayBeforeEnter);
                if (AreaScript.CurrentlyActiveBackgroundMusic == areaToTeleportTo.backgroundMusic)
                    skipPlay = true;
                else if (AreaScript.CurrentlyActiveBackgroundMusic != null && areaToTeleportTo.backgroundMusic != null)
                    skipPlay = AreaScript.CurrentlyActiveBackgroundMusic.clip == areaToTeleportTo.backgroundMusic.clip;
            }
            yield return new WaitForSeconds(delayBeforeEnter);
            LockableCamera.Instance.FreezeStateInCurrentPosition();
            player.transform.position = positionOnInteract;
            yield return new WaitForSeconds(delayJustAfterEnter);
            if (_animator != null)
                _animator.SetTrigger(CloseTrigger);
            yield return new WaitForSeconds(delayAfterDoorClose);
            if (areaToTeleportTo != null)
            {
                LockableCamera.Instance.SetState(ref areaToTeleportTo.cameraStateOnEnter);
                if (!skipPlay)
                    areaToTeleportTo.PlayBackgroundMusic();
            }

            player.movementEnabled = true;
            player.interactionEnabled = true;
        }
    }
}