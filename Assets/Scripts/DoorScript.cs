using Player;
using UnityEngine;
using Util;

namespace DefaultNamespace
{
    public class DoorScript : Interactable
    {
        public CameraState cameraStateOnInteract = CameraState.CreateMovingCameraState(Vector3.zero);
        public bool shouldPopCameraStateOnInteract = false;
        public Vector3 positionOnInteract;

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

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(positionOnInteract,0.5f);
        }
    }
}