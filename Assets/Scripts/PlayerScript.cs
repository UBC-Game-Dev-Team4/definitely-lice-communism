using DefaultNamespace;
using UnityEngine;
using Util;

namespace Player
{
    /// <summary>
    /// Script attached to a player object
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Rigidbody2D))]
    public class PlayerScript : MonoBehaviour
    {
        #region singleton
        /// <summary>
        /// Private single instance of <see cref="PlayerScript"/>
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Singleton_pattern"/>
        private static PlayerScript _player;
        /// <summary>
        /// Accessor to singleton instance of <see cref="PlayerScript"/>
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Singleton_pattern"/>
        public static PlayerScript Instance
        {
            get
            {
                if (_player == null) 
                    _player = FindObjectOfType<PlayerScript>();
                if (_player == null)
                {
                    Debug.LogWarning("Warning: player singleton is still null.");
                }
                return _player;
            }
        }
        #endregion
        [Tooltip("Whether debug data is drawn")]
        public bool debug = false;
        [Tooltip("Movement speed of player (excludes jumping)"), Min(0)]
        public float movementSpeed = 5;

        [Tooltip("Jumping velocity"),Min(0)]
        public float jumpSpeed = 10;
        [Tooltip("Movement speed multiplier when trying to change direction"),Min(0)]
        public float turnAroundMultiplier = 2;
        [Tooltip("Positional offset used in ground detection")]
        public Vector2 groundCheckOffset = new Vector2(0, 0.4f);
        [Tooltip("Size of ground detection box")]
        public Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);
        [Tooltip("Layers to query in ground detection")]
        public LayerMask groundCheckLayerMask;

        private InteractableDetector _detector;
        private Rigidbody2D _body;

        /// <summary>
        /// Locates required objects and sets singleton instance
        /// 
        /// Runs upon object being added/initialized, but before Start
        /// </summary>
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _detector = GetComponentInChildren<InteractableDetector>();
            _player = this;
        }

        /// <summary>
        /// Called once per frame - move the player.
        /// </summary>
        private void Update()
        {
            float multiplier = 1;
            if (_body.velocity.x * Input.GetAxis("Horizontal") < 0)
                multiplier = turnAroundMultiplier; // turn around faster
            _body.AddForce(new Vector2(movementSpeed * Time.deltaTime * Input.GetAxis("Horizontal") * multiplier, 0), ForceMode2D.Impulse);
            if (debug)
            {
                var position = transform.position;
                Debug.DrawLine(position,(Vector2)position+groundCheckOffset,Color.red);
            }
            if (Physics2D.OverlapBox((Vector2) transform.position + groundCheckOffset, groundCheckSize, 0,
                groundCheckLayerMask) != null && Input.GetKeyDown(SettingsManager.Instance.jumpKey))
            {
                _body.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(SettingsManager.Instance.interactKey))
            {
                _detector.Interact(this);
            }
        }

        /// <summary>
        /// On object destroy, remove singleton instance
        /// </summary>
        private void OnDestroy()
        {
            _player = null;
        }
    }
}