using ItemInventory;
using Player;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Item to define custom logic for an inventory item
    /// </summary>
    /// <seealso cref="ItemInventory.Item"/>
    [CreateAssetMenu(fileName = "Mallet Item", menuName = "Scene One/Mallet Item")]
    public class MalletItem : Item
    {
        [Tooltip("Width of area testing for chef")]
        public float killWidth = 1;
        [Tooltip("X offset of area testing for chef")]
        public float xOffset = 0;
        [Tooltip("X offset in direction of player facing")]
        public float xFacingOffset = 0.3f;
        [Tooltip("Height of area testing for chef")]
        public float killHeight = 0.4f;
        [Tooltip("Y Offset")]
        public float yOffset = 0.3f;
        /// <inheritdoc cref="ItemInventory.Item.Use"/>
        public override void Use()
        {
            Vector3 playerPos = PlayerScript.Instance.transform.position;
            bool isFacingLeft = PlayerScript.Instance.IsFacingLeft;
            float x = playerPos.x + xOffset + (isFacingLeft ? -xFacingOffset : xFacingOffset);
            float y = playerPos.y + yOffset;
            Vector2 center = new Vector2(x, y);
            Vector2 size = new Vector2(killWidth, killHeight);
#if UNITY_EDITOR
            Debug.DrawLine(new Vector3(center.x-size.x,center.y-size.y),new Vector3(center.x+size.x,center.y-size.y),Color.blue,5);
            Debug.DrawLine(new Vector3(center.x-size.x,center.y+size.y),new Vector3(center.x+size.x,center.y+size.y),Color.blue,5);
            Debug.DrawLine(new Vector3(center.x-size.x,center.y-size.y),new Vector3(center.x-size.x,center.y+size.y),Color.blue,5);
            Debug.DrawLine(new Vector3(center.x+size.x,center.y-size.y),new Vector3(center.x+size.x,center.y+size.y),Color.blue,5);
#endif
            Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, 0);
            foreach (Collider2D col in colliders)
            {
                ChefScript chef = col.GetComponentInChildren<ChefScript>();
                if (chef == null) chef = col.GetComponentInParent<ChefScript>();
                if (chef != null)
                {
                    Debug.Log("Heyo get murdered");
                    chef.Kill(false);
                }
            }
        }
    }
}