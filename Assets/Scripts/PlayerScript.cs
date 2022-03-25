using System;
using DefaultNamespace;
using Singleton;
using Sound;
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

        [Tooltip("Whether animation is updated")]
        public bool updateAnimation = true;

        [Tooltip("Footstep sounds. It's in the name.")]
        public AmbientSoundPlayer footstepSounds;

        /// <summary>
        /// Whether the player is currently facing left or right
        /// </summary>
        public bool IsFacingLeft
        {
            get;
            private set;
        }

        public Rigidbody2D Body { get; private set; }

        private InteractableDetector _detector;

        public Animator Animator { get; private set; }
        
        private static readonly int WalkSpeedParameter = Animator.StringToHash("WalkSpeedParameter");
        public static readonly int VelocityXParameter = Animator.StringToHash("VelocityX");

        /// <summary>
        ///     Locates required objects and sets singleton instance
        ///     Runs upon object being added/initialized, but before Start
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Body = GetComponent<Rigidbody2D>();
            _detector = GetComponentInChildren<InteractableDetector>();
            Animator = GetComponent<Animator>();
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
                if (Body.velocity.x * horizontalAxis < 0)
                    multiplier = turnAroundMultiplier; // turn around faster

                Body.AddForce(new Vector2(movementSpeed * Time.deltaTime * horizontalAxis * multiplier, 0), ForceMode2D.Impulse);
                if (debug)
                {
                    Vector3 position = transform.position;
                    Debug.DrawLine(position, (Vector2) position + groundCheckOffset, Color.red);
                }

                if (Physics2D.OverlapBox((Vector2) transform.position + groundCheckOffset, groundCheckSize, 0,
                    groundCheckLayerMask) != null && Input.GetKeyDown(SettingsManager.Instance.jumpKey))
                {
                    Body.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                    Animator.SetFloat(WalkSpeedParameter, 0);
                }
            }

            if (interactionEnabled && Input.GetKeyDown(SettingsManager.Instance.interactKey)) _detector.Interact(this);

            if (updateAnimation)
            {
                if (Math.Abs(Body.velocity.y) < 0.001f)
                {
                    footstepSounds.StartPlaying();
                    Animator.SetFloat(WalkSpeedParameter, 1);
                }
                else
                {
                    footstepSounds.StopPlaying();
                }


                if (Body.velocity.x < -0.01) IsFacingLeft = true;
                else if (Body.velocity.x > 0.01) IsFacingLeft = false;
                else
                {
                    footstepSounds.StopPlaying();
                    Animator.SetFloat(VelocityXParameter, IsFacingLeft ? -0.0001f : 0.0001f);
                }

                if (Body.velocity.x < -0.01 || Body.velocity.x > 0.01)
                {
                    footstepSounds.StartPlaying();
                    Animator.SetFloat(VelocityXParameter, Body.velocity.x);
                }
            }
        }

        /// <summary>
        /// Makes the player stationary.
        /// </summary>
        public void StopMoving()
        {
            Body.velocity = Vector2.zero;
            Animator.SetFloat(VelocityXParameter,0);
        }
    }
}