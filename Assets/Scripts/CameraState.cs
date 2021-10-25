using System;
using UnityEngine;

namespace Util
{
    [Serializable]
    public struct CameraState
    {
        public bool isStationary;

        public Vector3 positionOnLock;

        public Vector3 cameraOffset;

        public int cameraIdealWidth;
        public const float AspectRatio = 16f / 9f;

        public CameraState(bool isStationary, Vector3 positionOnLock, Vector3 cameraOffset, int cameraIdealWidth = 320)
        {
            this.isStationary = isStationary;
            this.positionOnLock = positionOnLock;
            this.cameraOffset = cameraOffset;
            this.cameraIdealWidth = cameraIdealWidth;
        }
        
        public static CameraState CreateStaticCameraState(Vector3 lockPosition, int cameraIdealWidth = 320)
        {
            return new CameraState(true, lockPosition, Vector3.zero, cameraIdealWidth);
        }

        public static CameraState CreateMovingCameraState(Vector3 offset, int cameraIdealWidth = 320)
        {
            return new CameraState(false, Vector3.zero, offset, cameraIdealWidth);
        }
    }
}