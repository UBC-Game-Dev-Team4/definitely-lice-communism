using DefaultNamespace;
using ItemInventory;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// AI script for a cat :3
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class CatAIScript : AIScript
    {
        [Tooltip("Item to make cat not run away")]
        public Item catFoodItem;
        
        [Tooltip("Whether the cat should run away")]
        public bool shouldRunAway = true;

        [Tooltip("Offset when calculating looking at player direction")]
        public float lookAtPlayerOffset = 0.25f;
        [Space]
        [Header("Sprites")]
        [Tooltip("Sprite of cat looking left")]
        public Sprite spriteLookLeft;
        [Tooltip("Sprite of cat looking right")]
        public Sprite spriteLookRight;
        [Tooltip("Sprite of cat not moving")]
        public Sprite spriteStationary;

        private SpriteRenderer _renderer;
        private CatInteractable _catInteractable;
        
        /// <inheritdoc cref="AIScript.Awake"/>
        protected override void Awake()
        {
            base.Awake();
            _catInteractable = GetComponentInChildren<CatInteractable>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        /// <inheritdoc cref="AIScript.FixedUpdate"/>
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            shouldRunAway = !Inventory.Instance.HasActiveItem(catFoodItem);
            _catInteractable.isInteractable = !shouldRunAway;
            if (mode == AIMode.Stationary) return;
            switch (currentSpeed > 0)
            {
                case true when directionIsLeft:
                    _renderer.sprite = spriteLookLeft;
                    break;
                case true when !directionIsLeft:
                    _renderer.sprite = spriteLookRight;
                    break;
                default:
                    _renderer.sprite = spriteStationary;
                    break;
            }
        }

        public void FacePlayer(Transform player)
        {
            if (player.transform.position.x + lookAtPlayerOffset < transform.position.x)
                _renderer.sprite = spriteLookLeft;
            else if (player.transform.position.x - lookAtPlayerOffset > transform.position.x)
                _renderer.sprite = spriteLookRight;
            else _renderer.sprite = spriteStationary;
        }

        public void SetRunAway(bool newValue, bool forceSet = false)
        {
            if (shouldRunAway == newValue && !forceSet) return;
            _catInteractable.isInteractable = !newValue;
        }
    }
}