using System;
using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class DoorScript : Interactable
    {
        public Vector3 positionOnInteract;
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            if (src is PlayerScript player)
            {
                player.transform.position = positionOnInteract;
            }
            
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(positionOnInteract,0.5f);
        }
    }
}