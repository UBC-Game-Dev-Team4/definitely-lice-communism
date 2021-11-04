﻿using System.Collections.Generic;
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

        private readonly Stack<CameraState> _states = new Stack<CameraState>();
        private PixelPerfectCamera _camera;
        private CameraState _currentActiveState;
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
            _states.Push(startingCameraState);
            ApplyState(startingCameraState);
            _currentActiveState = startingCameraState;
        }

        /// <summary>
        ///     Called once per frame - updates its position
        /// </summary>
        private void Update()
        {
            transform.position = _currentActiveState.GetCameraPos(_player.transform.position);
        }

        /// <summary>
        ///     On destroy, remove singleton instance
        /// </summary>
        private void OnDestroy()
        {
            _states.Clear();
            _instance = null;
        }

        /// <summary>
        ///     Adds a state to the list of states and sets that as current state
        /// </summary>
        /// <param name="state">New camera state</param>
        public void PushState(CameraState state)
        {
            _states.Push(state);
            _currentActiveState = state;
            ApplyState(state);
        }

        /// <summary>
        ///     Revert back to previous state, if none exists, go back to starting state
        /// </summary>
        public void PopState()
        {
            _states.Pop();
            if (_states.Count == 0) _states.Push(startingCameraState);
            _currentActiveState = _states.Peek();
            ApplyState(_currentActiveState);
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