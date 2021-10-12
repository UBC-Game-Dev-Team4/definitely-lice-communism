using Player;
using UnityEngine;

namespace Util {
    /// <summary>
    /// Script to be attached to the camera that can move with the player or with free camera enabled.
    /// </summary>
    /// <remarks>
    /// Doesn't actually require a camera - just should be attached to something that moves with the player but with
    /// the option of not moving with the player.
    /// </remarks>
    [DisallowMultipleComponent]
    public class CameraWithFreeLook : MonoBehaviour
    {
        /// <summary>
        /// Private single instance of <see cref="CameraWithFreeLook"/>
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Singleton_pattern"/>
        private static CameraWithFreeLook _instance;
        /// <summary>
        /// Accessor to singleton instance of <see cref="CameraWithFreeLook"/>
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Singleton_pattern"/>
        public static CameraWithFreeLook Instance
        {
            get
            {
                if (_instance != null) return _instance;
                if (Camera.main == null)
                {
                    Debug.LogWarning("Warning: Camera.main is null. Returning null.");
                }
                else
                {
                    _instance = Camera.main.transform.GetComponent<CameraWithFreeLook>();
                }

                return _instance;
            }
        }

        private SettingsManager _settings;
        private PlayerScript _player;
        [Tooltip("Offset of the camera when in free-look/free-cam mode")]
        public Vector3 camOffset = new Vector3(0,0,-10);
        [Tooltip("Determines if free-look/free-cam is enabled")]
        public bool isFreeLookEnabled = false;
        [Tooltip("Speed of moving camera"), Min(0)]
        public float speed = 10;

        /// <summary>
        /// Sets singleton instance, locates other required objects (player/settings), sets to initial position.
        ///
        /// Runs upon object being added/initialized, but before Start
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
                Debug.LogWarning("Warning: Existing GlobalCameraLocator. Rewriting...");
            _instance = this;
            _settings = SettingsManager.Instance;
            _player = PlayerScript.Instance;
            if (!isFreeLookEnabled)
            {
                transform.position = _player.transform.position + camOffset;
            }
        }
        
        /// <summary>
        /// Called once per frame - updates its position
        /// </summary>
        private void Update()
        {
            if (!_settings.toggleFreeLook)
            {
                if (Input.GetKeyDown(_settings.freeLookKey) || Input.GetKeyUp(_settings.freeLookKey))
                    isFreeLookEnabled = !isFreeLookEnabled;
            }
            else if (Input.GetKeyDown(_settings.freeLookKey))
            {
                isFreeLookEnabled = !isFreeLookEnabled;
            }

            if (!isFreeLookEnabled)
            {
                transform.position = _player.transform.position + camOffset;
            }
            else
            {
                transform.position += new Vector3(speed * Input.GetAxis("Horizontal") * Time.deltaTime, speed * Input.GetAxis("Vertical") * Time.deltaTime, 0);
            }
        }

        /// <summary>
        /// On destroy, remove singleton instance
        /// </summary>
        private void OnDestroy()
        {
            _instance = null;
        }
    }
}