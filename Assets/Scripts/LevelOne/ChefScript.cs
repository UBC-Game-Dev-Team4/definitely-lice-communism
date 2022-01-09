using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace LevelOne
{
    [RequireComponent(typeof(Animator),typeof(SpriteRenderer))]
    public class ChefScript : AIScript
    {
        public DoorScript kitchenDoorTarget;
        public float doorTargetOffset;
        public DoorScript backroomDoorTarget;
        public float secondRoomDoorTargetOffset;
        public float secondRoomTarget;
        public float delayAfterBackroomTarget = 3;
        public float kitchenResetTarget;
        public float delayBeforeTeleport = 1;
        public float delayAfterTeleport = 1;
        private SpriteRenderer _renderer;
        private ChefState _state = ChefState.Still;
        private Animator _animator;
        private static readonly int StationaryTrigger = Animator.StringToHash("StationaryTrigger");
        private static readonly int WalkTrigger = Animator.StringToHash("WalkTrigger");

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            SetMode(AIMode.Stationary);
        }

        private void SetCorrectSpriteRendererFlip()
        {
            if (_state != ChefState.Still)
            {
                _renderer.flipX = targetX > transform.position.x;
            }
        }

        public void MoveFromKitchen()
        {
            if (_state == ChefState.Still)
            {
                shouldBeMoving = true;
                _animator.SetTrigger(WalkTrigger);
                targetX = kitchenDoorTarget.transform.position.x + doorTargetOffset;
                _state = ChefState.MovingToKitchenDoor;
                SetMode(AIMode.SpecificX);
                SetCorrectSpriteRendererFlip();
            }
        }

        public void StopMoving()
        {
            _animator.SetTrigger(StationaryTrigger);
            SetMode(AIMode.Stationary);
            _state = ChefState.Still;
            shouldBeMoving = false;
        }

        protected override void OnReachSpecificXDestination()
        {
            base.OnReachSpecificXDestination();
            switch (_state)
            {
                case ChefState.MovingToBackroomDoor:
                case ChefState.MovingToKitchenDoor:
                    StartCoroutine(nameof(TeleportAndMoveAfterDoorOpen));
                    break;
                case ChefState.MovingToKitchenReset:
                    StopMoving();
                    break;
                case ChefState.MovingToBackroomTarget:
                    StartCoroutine(nameof(OnBackroomTargetReachedCoroutine));
                    break;
            }
        }

        private IEnumerator OnBackroomTargetReachedCoroutine()
        {
            shouldBeMoving = false;
            SetMode(AIMode.Stationary);
            yield return new WaitForSeconds(delayAfterBackroomTarget);
            shouldBeMoving = true;
            targetX = backroomDoorTarget.transform.position.x + secondRoomDoorTargetOffset;
            _state = ChefState.MovingToBackroomDoor;
            SetMode(AIMode.SpecificX);
            SetCorrectSpriteRendererFlip();
        }

        private IEnumerator TeleportAndMoveAfterDoorOpen()
        {
            shouldBeMoving = false;
            SetMode(AIMode.Stationary);
            yield return new WaitForSeconds(delayBeforeTeleport);
            shouldBeMoving = true;
            if (_state == ChefState.MovingToKitchenDoor)
            {
                body.position = kitchenDoorTarget.positionOnInteract;
                targetX = secondRoomTarget;
                _state = ChefState.MovingToBackroomTarget;
            }
            else
            {
                body.position = backroomDoorTarget.positionOnInteract;
                targetX = kitchenResetTarget;
                _state = ChefState.MovingToKitchenReset;
            }

            yield return new WaitForSeconds(delayAfterTeleport);
            SetMode(AIMode.SpecificX);
            SetCorrectSpriteRendererFlip();
        }

        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Vector3 doorTargetPos = kitchenDoorTarget.transform.position;
            Vector3 secondDoorPos = backroomDoorTarget.transform.position;
            
            Gizmos.DrawWireSphere(new Vector3(doorTargetPos.x + doorTargetOffset, doorTargetPos.y, doorTargetPos.z),1);
            Gizmos.DrawWireSphere(new Vector3(secondDoorPos.x + secondRoomDoorTargetOffset, secondDoorPos.y, secondDoorPos.z),1);
            Gizmos.DrawWireSphere(new Vector3(secondRoomTarget,secondDoorPos.y,secondDoorPos.z),1);
            Gizmos.DrawWireSphere(new Vector3(kitchenResetTarget,doorTargetPos.y,doorTargetPos.z),1);
        }

        private enum ChefState
        {
            Still,
            MovingToBackroomDoor,
            MovingToBackroomTarget,
            MovingToKitchenDoor,
            MovingToKitchenReset
        }
    }
}