using Player;
using UnityEngine;
using Util;

namespace DefaultNamespace
{
    /// <summary>
    /// Script attached to an interactable door
    /// </summary>
    public class DoorScript : Interactable
    {
        [Tooltip("Camera state to set camera to upon interaction with this object")]
        public CameraState cameraStateOnInteract = CameraState.CreateMovingCameraState(Vector3.zero);
        [Tooltip("Whether the camera state should revert to previous state")]
        public bool shouldPopCameraStateOnInteract;
        [Tooltip("Position to teleport player on interaction")]
        public Vector3 positionOnInteract;

        /// <inheritdoc cref="Interactable"/>
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            if (!(src is PlayerScript player)) return;
            if (shouldPopCameraStateOnInteract)
                LockableCamera.Instance.PopState();
            else
                LockableCamera.Instance.PushState(cameraStateOnInteract);
            player.transform.position = positionOnInteract;
        }

        /// <summary>
        /// Draw a wireframe sphere around the position to teleport to
        /// </summary>
        /// <inheritdoc cref="MonoBehaviour"/>
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(positionOnInteract,0.5f);
        }
    }
}