using Player;
using UnityEngine;
using UnityEngine.U2D;

namespace Util
{
    /// <summary>
    ///     Script to be attached to the camera that can move with the player or with free camera enabled.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PixelPerfectCamera))]
    public class LockableCamera : MonoBehaviour
    {
        /// <summary>
        ///     Private single instance of <see cref="LockableCamera" />
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Singleton_pattern" />
        private static LockableCamera _instance;

        [Tooltip("Camera state to start at")]
        public CameraState startingCameraState = CameraState.CreateFreeXYCameraState(new Vector3(0, 0, -10));

        private PixelPerfectCamera _camera;
        private CameraState _cameraState;
        private PlayerScript _player;

        /// <summary>
        ///     Accessor to singleton instance of <see cref="LockableCamera" />
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Singleton_pattern" />
        public static LockableCamera Instance
        {
            get
            {
                if (_instance != null) return _instance;
                if (Camera.main == null)
                    Debug.LogWarning("Warning: Camera.main is null. Returning null.");
                else
                    _instance = Camera.main.transform.GetComponent<LockableCamera>();

                return _instance;
            }
        }

        /// <summary>
        ///     Sets singleton instance, locates other required objects (player/settings), sets to initial position.
        ///     Runs upon object being added/initialized, but before Start
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
                Debug.LogWarning("Warning: Existing GlobalCameraLocator. Rewriting...");
            _instance = this;
            _camera = GetComponent<PixelPerfectCamera>();
            _player = PlayerScript.Instance;
            _cameraState = startingCameraState;
            ApplyState(startingCameraState);
        }

        /// <summary>
        ///     Called once per frame - updates its position
        /// </summary>
        private void Update()
        {
            transform.position = _cameraState.GetCameraPos(_player.transform.position);
        }

        /// <summary>
        ///     On destroy, remove singleton instance
        /// </summary>
        private void OnDestroy()
        {
            _instance = null;
        }

        /// <summary>
        /// Sets the camera state to be this new state
        /// </summary>
        /// <param name="state">New camera state</param>
        public void SetState(ref CameraState state)
        {
            _cameraState = state.Copy();
            ApplyState(state);
        }

        /// <summary>
        /// Sets the camera state to be a new state where the camera is frozen at its current location
        /// </summary>
        public void FreezeStateInCurrentPosition()
        {
            Vector3 position = transform.position;
            Vector3 offset = _cameraState.cameraOffset;
            float newX = position.x - offset.x;
            float newY = position.y - offset.y;
            _cameraState = CameraState.CreateFixedXYCameraState(newX, newY, offset,_cameraState.cameraIdealWidth);
            ApplyState(_cameraState);
        }

        /// <summary>
        ///     Applies a given state's settings (i.e. position + offset/camera res)
        /// </summary>
        /// <param name="state">State settings to apply</param>
        private void ApplyState(CameraState state)
        {
            transform.position = state.GetCameraPos(_player.transform.position);
            _camera.refResolutionX = state.cameraIdealWidth;
            _camera.refResolutionY = Mathf.FloorToInt(state.cameraIdealWidth * 1 / CameraState.AspectRatio);
        }
    }
}