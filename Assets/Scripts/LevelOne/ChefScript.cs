using System.Collections;
using DefaultNamespace;
using Sound;
using UnityEngine;
using Util;

namespace LevelOne
{
    /// <summary>
    /// Script that controls the Chef in Level 1 (when alive)
    /// </summary>
    [RequireComponent(typeof(Animator),typeof(SpriteRenderer), typeof(AmbientSoundPlayer))]
    public class ChefScript : AIScript
    {
        [Tooltip("Prefab of the dead chef")]
        public GameObject deadChefPrefab;
        [Tooltip("Offset of spawn relative to current position of dead chef")]
        public Vector3 deadSpawnOffset;
        [Tooltip("Door in kitchen to move towards")]
        public DoorScript kitchenDoorTarget;
        [Tooltip("X Offset of position of door in kitchen to move towards")]
        public float doorTargetOffset;
        [Tooltip("Door in storage room to move towards")]
        public DoorScript storageRoomTarget;
        [Tooltip("X offset of door in storage room to move towards")]
        public float secondRoomDoorTargetOffset;
        [Tooltip("After entering door in kitchen, x position of target in storage room to move towards")]
        public float secondRoomTarget;
        [Tooltip("Delay after reaching target in storage room")]
        public float delayAfterStorageRoomTarget = 3;
        [Tooltip("X position of point in kitchen to reset position to")]
        public float kitchenResetTarget;
        [Tooltip("Delay before teleporting through a door")]
        public float delayBeforeTeleport = 1;
        [Tooltip("Delay after teleporting through a door")]
        public float delayAfterTeleport = 1;
        private SpriteRenderer _renderer;
        private AmbientSoundPlayer _player;
        private ChefState _state = ChefState.Still;
        private Animator _animator;
        private static readonly int StationaryTrigger = Animator.StringToHash("StationaryTrigger");
        private static readonly int WalkTrigger = Animator.StringToHash("WalkTrigger");
        public static ChefScript InstanceIfPresent { get; private set; }

        
        protected override void Awake()
        {
            base.Awake();
            InstanceIfPresent = this;
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _player = GetComponent<AmbientSoundPlayer>();
            TargetXReached += OnReachSpecificXDestination;
            SetMode(AIMode.Stationary);
        }

        private void SetCorrectSpriteRendererFlip()
        {
            if (_state != ChefState.Still)
            {
                _renderer.flipX = targetX > transform.position.x;
            }
        }

        /// <summary>
        /// Kill the chef by creating a prefab and signaling the LevelOneInfoStorer about the dead chef's position
        /// </summary>
        public void Kill(bool isViaHotOil)
        {
            Transform thisTransform = transform;
            Vector3 newSpawn = thisTransform.localPosition + deadSpawnOffset;
            GameObject go = Instantiate(deadChefPrefab, thisTransform.parent);
            go.transform.localPosition = newSpawn;
            Destroy(gameObject);
            LevelOneInfoStorer.CastedSingleton.OnKilledChef(go.transform.position);
            LevelOneInfoStorer.CastedSingleton.CastedInfo.AddMurderRespect(30);
            LevelOneInfoStorer.CastedSingleton.CastedInfo.wasKilledViaHotOil = isViaHotOil;
            _player.StopPlaying();
        }

        /// <summary>
        /// Begins the process of moving the chef from the kitchen to the storage room and back, if not already doing so
        /// </summary>
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
                _player.StopPlaying();
            }
        }

        /// <summary>
        /// Makes the chef stand still and chop
        /// </summary>
        public void StopMoving()
        {
            _animator.SetTrigger(StationaryTrigger);
            SetMode(AIMode.Stationary);
            _state = ChefState.Still;
            shouldBeMoving = false;
            if (AreaScript.currentArea == kitchenDoorTarget.currentArea)
            {
                _player.StartPlaying();
            }
        }

        private void OnReachSpecificXDestination(float ignoredTarget)
        {
            switch (_state)
            {
                case ChefState.MovingToStorageRoomDoor:
                case ChefState.MovingToKitchenDoor:
                    StartCoroutine(nameof(TeleportAndMoveAfterDoorOpen));
                    break;
                case ChefState.MovingToKitchenReset:
                    StopMoving();
                    break;
                case ChefState.MovingToStorageRoomTarget:
                    StartCoroutine(nameof(OnStorageRoomTargetReachedCoroutine));
                    break;
            }
        }

        private IEnumerator OnStorageRoomTargetReachedCoroutine()
        {
            shouldBeMoving = false;
            SetMode(AIMode.Stationary);
            yield return new WaitForSeconds(delayAfterStorageRoomTarget);
            shouldBeMoving = true;
            targetX = storageRoomTarget.transform.position.x + secondRoomDoorTargetOffset;
            _state = ChefState.MovingToStorageRoomDoor;
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
                _state = ChefState.MovingToStorageRoomTarget;
            }
            else
            {
                body.position = storageRoomTarget.positionOnInteract;
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
            Vector3 secondDoorPos = storageRoomTarget.transform.position;
            
            Gizmos.DrawWireSphere(new Vector3(doorTargetPos.x + doorTargetOffset, doorTargetPos.y, doorTargetPos.z),1);
            Gizmos.DrawWireSphere(new Vector3(secondDoorPos.x + secondRoomDoorTargetOffset, secondDoorPos.y, secondDoorPos.z),1);
            Gizmos.DrawWireSphere(new Vector3(secondRoomTarget,secondDoorPos.y,secondDoorPos.z),1);
            Gizmos.DrawWireSphere(new Vector3(kitchenResetTarget,doorTargetPos.y,doorTargetPos.z),1);
        }

        private enum ChefState
        {
            Still,
            MovingToStorageRoomDoor,
            MovingToStorageRoomTarget,
            MovingToKitchenDoor,
            MovingToKitchenReset
        }
    }
}