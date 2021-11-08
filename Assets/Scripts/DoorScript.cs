using Player;
using UnityEngine;
using Util;

namespace DefaultNamespace
{
    /// <summary>
    ///     Script attached to an interactable door
    /// </summary>
    public class DoorScript : Interactable
    {
        [Tooltip("Position to teleport player on interaction")]
        public Vector3 positionOnInteract;

        public AreaScript areaToTeleportTo;
        
        /// <summary>
        ///     Draw a wireframe sphere around the position to teleport to
        /// </summary>
        /// <inheritdoc cref="MonoBehaviour" />
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(positionOnInteract, 0.5f);
        }

        /// <inheritdoc cref="Interactable" />
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            if (!(src is PlayerScript player)) return;
            if (areaToTeleportTo != null)
            {
                LockableCamera.Instance.SetState(ref areaToTeleportTo.cameraStateOnEnter);
            }

            player.transform.position = positionOnInteract;
            player.StopMoving();
        }
    }
}