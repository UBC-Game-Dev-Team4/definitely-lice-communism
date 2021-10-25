using System;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// State of a Camera
    /// </summary>
    [Serializable]
    public struct CameraState
    {
        [Tooltip("Whether the camera is locked in place")]
        public bool isStationary;
        [Tooltip("Position to lock the camera to")]
        public Vector3 positionOnLock;
        [Tooltip("Offset to player for moving camera")]
        public Vector3 cameraOffset;
        /// <summary>
        /// <see cref="UnityEngine.U2D.PixelPerfectCamera"/>'s reference width
        /// </summary>
        [Tooltip("Pixel Perfect Camera's reference width")]
        public int cameraIdealWidth;
        /// <summary>
        /// Width to height ratio
        /// </summary>
        public const float AspectRatio = 16f / 9f;

        /// <summary>
        /// Create a CameraState with given parameters
        /// </summary>
        /// <param name="isStationary">Whether the camera is locked in place</param>
        /// <param name="positionOnLock">Position to lock camera to</param>
        /// <param name="cameraOffset">Offset to player for moving camera</param>
        /// <param name="cameraIdealWidth">Refer to <see cref="CameraState.cameraIdealWidth"/></param>
        public CameraState(bool isStationary, Vector3 positionOnLock, Vector3 cameraOffset, int cameraIdealWidth = 320)
        {
            this.isStationary = isStationary;
            this.positionOnLock = positionOnLock;
            this.cameraOffset = cameraOffset;
            this.cameraIdealWidth = cameraIdealWidth;
        }
        
        /// <summary>
        /// Create a locked CameraState with given lock position and camera view width
        /// </summary>
        /// <param name="lockPosition">Position to lock camera to</param>
        /// <param name="cameraIdealWidth">Width of camera view</param>
        /// <returns>CameraState with given parameters</returns>
        public static CameraState CreateStaticCameraState(Vector3 lockPosition, int cameraIdealWidth = 320)
        {
            return new CameraState(true, lockPosition, Vector3.zero, cameraIdealWidth);
        }

        /// <summary>
        /// Create a mobile CameraState with given offset and camera view width
        /// </summary>
        /// <param name="offset">Offset to player of camera</param>
        /// <param name="cameraIdealWidth">Camera view width</param>
        /// <returns>CameraState with given parameters</returns>
        public static CameraState CreateMovingCameraState(Vector3 offset, int cameraIdealWidth = 320)
        {
            return new CameraState(false, Vector3.zero, offset, cameraIdealWidth);
        }
    }
}