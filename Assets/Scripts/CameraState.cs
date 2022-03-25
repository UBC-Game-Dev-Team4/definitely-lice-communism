using System;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// The three possible states of movement: Fixed (can't move), Range (free within a range), Free (no range)
    /// </summary>
    [Serializable]
    public enum CameraMovementMode
    {
        Fixed, Range, Free
    }
    /// <summary>
    ///     State of a Camera
    /// </summary>
    [Serializable]
    public struct CameraState
    {
        /// <summary>
        ///     Width to height ratio
        /// </summary>
        public const float AspectRatio = 16f / 9f;

        [Tooltip("Movement mode along X axis")]
        public CameraMovementMode movementModeX;
        
        [Tooltip("If movement mode is fixed, position to fix at (else ignored)")]
        public float movementFixedX;

        [Tooltip("If movement mode is range, lower bound of range (else ignored)")]
        public float movementRangeXLeft;

        [Tooltip("If movement mode is range, upper bound of range (else ignored)")]
        public float movementRangeXRight;
        
        [Tooltip("Movement mode along Y axis")]
        public CameraMovementMode movementModeY;
        
        [Tooltip("If movement mode is fixed, position to fix at (else ignored)")]
        public float movementFixedY;

        [Tooltip("If movement mode is range, lower bound of range (else ignored)")]
        public float movementRangeYLeft;

        [Tooltip("If movement mode is range, upper bound of range (else ignored)")]
        public float movementRangeYRight;
        
        [Tooltip("Offset to player for camera")]
        public Vector3 cameraOffset;

        /// <summary>
        ///     <see cref="UnityEngine.U2D.PixelPerfectCamera" />'s reference width
        /// </summary>
        [Tooltip("Pixel Perfect Camera's reference width")]
        public int cameraIdealWidth;

        /// <summary>
        /// Constructor with all possible values.
        /// </summary>
        /// <remarks>
        /// For your own sanity, create a static method that calls this instead of using this in your code everywhere.
        /// </remarks>
        internal CameraState(CameraMovementMode movementModeX, float movementFixedX, float movementRangeXLeft, float movementRangeXRight,
            CameraMovementMode movementModeY, float movementFixedY, float movementRangeYLeft, float movementRangeYRight, Vector3 cameraOffset, int cameraIdealWidth = 320)
        {
            this.movementModeX = movementModeX;
            this.movementFixedX = movementFixedX;
            this.movementRangeXLeft = movementRangeXLeft;
            this.movementRangeXRight = movementRangeXRight;
            this.movementModeY = movementModeY;
            this.movementFixedY = movementFixedY;
            this.movementRangeYLeft = movementRangeYLeft;
            this.movementRangeYRight = movementRangeYRight;
            this.cameraOffset = cameraOffset;
            this.cameraIdealWidth = cameraIdealWidth;
            Debug.Assert(movementRangeYRight >= movementRangeYLeft);
            Debug.Assert(movementRangeXRight >= movementRangeXLeft);
        }

        /// <summary>
        /// Given an input position, return the desired camera position.
        /// </summary>
        /// <param name="position">Input position</param>
        /// <returns>Desired camera position</returns>
        public Vector3 GetCameraPos(in Vector3 position)
        {
            Vector3 newPosition = new Vector3(movementFixedX,movementFixedY);
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (movementModeX)
            {
                case CameraMovementMode.Free:
                    newPosition.x = position.x;
                    break;
                case CameraMovementMode.Range:
                {
                    if (position.x < movementRangeXLeft)
                        newPosition.x = movementRangeXLeft;
                    else if (position.x >= movementRangeXLeft && position.x <= movementRangeXRight)
                        newPosition.x = position.x;
                    else newPosition.x = movementRangeXRight;
                    break;
                }
            }

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (movementModeY)
            {
                case CameraMovementMode.Free:
                    newPosition.y = position.y;
                    break;
                case CameraMovementMode.Range:
                {
                    if (position.y < movementRangeYLeft)
                        newPosition.y = movementRangeYLeft;
                    else if (position.y >= movementRangeYLeft && position.y <= movementRangeYRight)
                        newPosition.y = position.y;
                    else newPosition.y = movementRangeYRight;
                    break;
                }
            }
            return newPosition + cameraOffset;
        }

        /// <summary>
        /// Returns a CameraState with unrestricted X and Y values
        /// </summary>
        /// <param name="cameraOffset">Camera Offset</param>
        /// <param name="cameraIdealWidth">Ideal camera view width</param>
        /// <returns>CameraState with given options</returns>
        public static CameraState CreateFreeXYCameraState(Vector3 cameraOffset, int cameraIdealWidth = 320)
        {
            return new CameraState(CameraMovementMode.Free, 0, 0, 0, CameraMovementMode.Free, 0, 0, 0, cameraOffset,
                cameraIdealWidth);
        }

        /// <summary>
        /// Returns a CameraState with a fixed Y value but an X value between a range
        /// </summary>
        /// <param name="leftX">Left bound on X</param>
        /// <param name="rightX">Right bound on X</param>
        /// <param name="fixedY">Fixed value for Y</param>
        /// <param name="cameraOffset">Camera Offset</param>
        /// <param name="cameraIdealWidth">Ideal Camera view Width</param>
        /// <returns>CameraState with given options</returns>
        public static CameraState CreateRangeXFixedYCameraState(float leftX, float rightX, float fixedY,
            Vector3 cameraOffset, int cameraIdealWidth = 320)
        {
            return new CameraState(CameraMovementMode.Range, 0, leftX, rightX, CameraMovementMode.Fixed, fixedY, 0, 0,
                cameraOffset, cameraIdealWidth);
        }

        /// <summary>
        /// Returns a CameraState with fixed X and Y
        /// </summary>
        /// <param name="movementFixedX">Fixed X position</param>
        /// <param name="movementFixedY">Fixed Y position</param>
        /// <param name="cameraOffset">Camera Offset</param>
        /// <param name="cameraIdealWidth">Ideal Camera View width</param>
        /// <returns>CameraState with given options</returns>
        public static CameraState CreateFixedXYCameraState(float movementFixedX, float movementFixedY, Vector3 cameraOffset, int cameraIdealWidth = 320)
        {
            return new CameraState(CameraMovementMode.Fixed, movementFixedX, 0, 0, CameraMovementMode.Fixed,
                movementFixedY, 0, 0, cameraOffset, cameraIdealWidth);
        }

        public CameraState Copy() =>
            new CameraState(movementModeX, movementFixedX, movementRangeXLeft, movementRangeXRight,
                movementModeY, movementFixedY, movementRangeYLeft, movementRangeYRight, cameraOffset,
                cameraIdealWidth);
    }
}