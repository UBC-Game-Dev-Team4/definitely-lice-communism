using System;
using DefaultNamespace;
using Singleton;
using UnityEngine;
using Util;

namespace Player
{
    /// <summary>
    ///     Script attached to a player object
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
    public class PlayerScript : DestroySingleton<PlayerScript>
    {
        [Tooltip("Whether debug data is drawn")]
        public bool debug;

        [Tooltip("Movement speed of player (excludes jumping)")] [Min(0)]
        public float movementSpeed = 5;

        [Tooltip("Jumping velocity")] [Min(0)] public float jumpSpeed = 10;

        [Tooltip("Movement speed multiplier when trying to change direction")] [Min(0)]
        public float turnAroundMultiplier = 2;

        [Tooltip("Positional offset used in ground detection")]
        public Vector2 groundCheckOffset = new Vector2(0, 0.4f);

        [Tooltip("Size of ground detection box")]
        public Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);

        [Tooltip("Layers to query in ground detection")]
        public LayerMask groundCheckLayerMask;

        [Tooltip("Whether movement is disabled")]
        public bool movementEnabled = true;

        [Tooltip("Whether interaction is disabled")]
        public bool interactionEnabled = true;

        private Rigidbody2D _body;

        private InteractableDetector _detector;

        private Animator _animator;
        private bool _isFacingLeft;
        
        private static readonly int WalkSpeedParameter = Animator.StringToHash("WalkSpeedParameter");
        private static readonly int VelocityXParameter = Animator.StringToHash("VelocityX");

        /// <summary>
        ///     Locates required objects and sets singleton instance
        ///     Runs upon object being added/initialized, but before Start
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _body = GetComponent<Rigidbody2D>();
            _detector = GetComponentInChildren<InteractableDetector>();
            _animator = GetComponent<Animator>();
            instance = this;
        }

        /// <summary>
        ///     Called once per frame - move the player.
        /// </summary>
        private void Update()
        {
            if (movementEnabled)
            {
                float multiplier = 1;
                float horizontalAxis = Input.GetAxis("Horizontal");
                if (_body.velocity.x * horizontalAxis < 0)
                    multiplier = turnAroundMultiplier; // turn around faster

                _body.AddForce(new Vector2(movementSpeed * Time.deltaTime * horizontalAxis * multiplier, 0), ForceMode2D.Impulse);
                if (debug)
                {
                    Vector3 position = transform.position;
                    Debug.DrawLine(position, (Vector2) position + groundCheckOffset, Color.red);
                }

                if (Physics2D.OverlapBox((Vector2) transform.position + groundCheckOffset, groundCheckSize, 0,
                    groundCheckLayerMask) != null && Input.GetKeyDown(SettingsManager.Instance.jumpKey))
                {
                    _body.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                    _animator.SetFloat(WalkSpeedParameter, 0);
                }
            }

            if (interactionEnabled && Input.GetKeyDown(SettingsManager.Instance.interactKey)) _detector.Interact(this);

            if (Math.Abs(_body.velocity.y) < 0.001f)
                _animator.SetFloat(WalkSpeedParameter,1);
            
            
            if (_body.velocity.x < -0.01) _isFacingLeft = true;
            else if (_body.velocity.x > 0.01) _isFacingLeft = false;
            else
            {
                _animator.SetFloat(VelocityXParameter, _isFacingLeft ? -0.0001f : 0.0001f);
            }
            if (_body.velocity.x < -0.01 || _body.velocity.x > 0.01)
                _animator.SetFloat(VelocityXParameter,_body.velocity.x);
        }

        /// <summary>
        /// Makes the player stationary.
        /// </summary>
        public void StopMoving()
        {
            _body.velocity = Vector2.zero;
            _animator.SetFloat(VelocityXParameter,0);
        }
    }
}