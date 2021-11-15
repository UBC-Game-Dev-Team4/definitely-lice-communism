
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// AI script for a cat :3
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class CatAIScript : AIScript
    {
        [Space]
        [Header("Sprites")]
        [Tooltip("Sprite of cat looking left")]
        public Sprite spriteLookLeft;
        [Tooltip("Sprite of cat looking right")]
        public Sprite spriteLookRight;
        [Tooltip("Sprite of cat not moving")]
        public Sprite spriteStationary;
        private SpriteRenderer _renderer;
        
        /// <inheritdoc cref="AIScript.Awake"/>
        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponent<SpriteRenderer>();
        }

        /// <inheritdoc cref="AIScript.FixedUpdate"/>
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
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
    }
}