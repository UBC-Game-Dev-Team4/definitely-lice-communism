using DefaultNamespace;
using ItemInventory;
using JetBrains.Annotations;
using Sound;
using UnityEngine;

namespace LevelOne.Cat
{
    /// <summary>
    /// AI script for a cat :3
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer),typeof(Animator))]
    public class CatAIScript : AIScript
    {
        [Tooltip("Item to make cat not run away")]
        public Item catFoodItem;
        
        [Tooltip("Whether the cat should run away")]
        public bool shouldRunAway = true;

        [Tooltip("Offset when calculating looking at player direction")]
        public float lookAtPlayerOffset = 0.25f;

        [Tooltip("X velocity before movement animation plays")]
        public float velMovementThreshold = 0.01f;
        
        [Tooltip("Animation velocity parameter when stationary")]
        public float velLookValue = 0.00001f;

        [Tooltip("Delay before bending over occurs")]
        public float delayBeforeBendOver = 1;

        [Tooltip("Key ambient noises")]
        public AmbientSoundPlayer keyAmbient;
        [Tooltip("Normal ambient noises")]
        public AmbientSoundPlayer normalAmbient;
        /// <summary>
        /// Whether the key was taken or not
        /// </summary>
        public bool KeyTaken { get; private set; }

        private Animator _animator;
        private SpriteRenderer _renderer;
        private CatInteractable _catInteractable;
        private bool _isFacingLeft;
        private bool _isEating;
        private static readonly int VelX = Animator.StringToHash("VelX");
        private static readonly int RemoveKey = Animator.StringToHash("RemoveKey");
        private static readonly int StartEat = Animator.StringToHash("StartEat");
        private static readonly int BendOver = Animator.StringToHash("BendOver");

        /// <inheritdoc cref="AIScript.Awake"/>
        protected override void Awake()
        {
            base.Awake();
            _catInteractable = GetComponentInChildren<CatInteractable>();
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            CatFoodItem.OnPlaceEvent += OnPlaceCatFood;
        }

        /// <inheritdoc cref="AIScript.FixedUpdate"/>
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            shouldRunAway = !Inventory.Instance.HasActiveItem(catFoodItem);
            bool isRunning = false;
            if (currentSpeed > velMovementThreshold)
            {
                _isFacingLeft = directionIsLeft;
                _animator.SetFloat(VelX,_isFacingLeft? -currentSpeed : currentSpeed);
                isRunning = true;
            } else
            {
                if (_isFacingLeft)
                    _animator.SetFloat(VelX, -velLookValue);
                else
                    _animator.SetFloat(VelX, velLookValue);
            }
            
            _renderer.flipX = !_isFacingLeft;
            // Run animation is in... opposite direction of the idle animation
            if (isRunning) _renderer.flipX = !_renderer.flipX;
        }

        /// <summary>
        /// Stop playing normal/key ambient audio
        /// </summary>
        public void StopPlayingAudio()
        {
            normalAmbient.StopPlaying();
            keyAmbient.StopPlaying();
        }

        /// <summary>
        /// Start playing audio
        /// </summary>
        public void StartPlayingAudio()
        {
            normalAmbient.StartPlaying();
            if (!KeyTaken) keyAmbient.StartPlaying();
        }

        public void FacePlayer(Transform player)
        {
            if (player.transform.position.x + lookAtPlayerOffset < transform.position.x)
                _isFacingLeft = true;
            else if (player.transform.position.x - lookAtPlayerOffset > transform.position.x)
                _isFacingLeft = false;
        }

        #region various animation/events
        private void OnPlaceCatFood()
        {
            targetX = CatFoodItem.PlacedLocation.transform.position.x;
            targetXTolerance = 0.001f;
            SetMode(AIMode.SpecificX);
            CatFoodItem.OnPlaceEvent -= OnPlaceCatFood;
        }
        public void OnReachCatFood()
        {
            SetMode(AIMode.Stationary);
            Invoke(nameof(BeginBendOver),delayBeforeBendOver);
        }
        private void BeginBendOver()
        {
            _animator.SetTrigger(BendOver);
        }
        [UsedImplicitly]
        public void OnBendOverEnd()
        {
            _animator.SetTrigger(StartEat);
            _catInteractable.isInteractable = true;
            _catInteractable.priority += _catInteractable.priorityChangeWhenInteractable;
        }
        
        public void OnKeyTaken()
        {
            _animator.SetTrigger(RemoveKey);
            KeyTaken = true;
        }
        #endregion

        private void OnDestroy()
        {
            CatFoodItem.OnPlaceEvent -= OnPlaceCatFood;
        }
    }
}