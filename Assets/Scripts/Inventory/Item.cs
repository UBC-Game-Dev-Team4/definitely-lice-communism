using UnityEngine;

namespace ItemInventory
{
    /// <summary>
    ///     Represents an item.
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        public new string name = "New Item";
        public Sprite icon;
        [TextArea]
        public string description = "";
        public bool isDefaultItem;

        public override bool Equals(object other)
        {
            if (!(other is Item item)) return false;
            if (item == null) return false;
            return name.Equals(item.name) && icon.Equals(item.icon) && isDefaultItem == item.isDefaultItem && description.Equals(item.description);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = 31 * hash + (name == null ? 0 : name.GetHashCode());
            hash = 31 * hash + (icon == null ? 0 : icon.GetHashCode());
            hash = 31 * hash + (description == null ? 0 : description.GetHashCode());
            return 31 * hash + (isDefaultItem ? 1 : 0);
        }
    }
}