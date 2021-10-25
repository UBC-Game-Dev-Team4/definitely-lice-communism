using UnityEngine;
using UnityEngine.UI;

namespace ItemInventory
{
    /// <summary>
    ///     Script for an inventory slot.
    /// </summary>
    /// <remarks>
    ///     Based on Brackeys INVENTORY CODE - Making an RPG in Unity (E06)
    /// </remarks>
    public class InventorySlot : MonoBehaviour
    {
        [Tooltip("Displayed icon.")] public Image icon;

        [Tooltip("Button to remove item.")] public Button removeButton;

        private Item _item;

        /// <summary>
        ///     Adds the item to the slot.
        /// </summary>
        /// <param name="newItem"></param>
        public void AddItem(Item newItem)
        {
            _item = newItem;
            icon.color = new Color(255, 255, 255, 255);
            icon.sprite = _item.icon;
            icon.enabled = true;
            removeButton.interactable = true;
        }

        /// <summary>
        ///     Clears the icon from the slot and the item.
        /// </summary>
        public void ClearSlot()
        {
            _item = null;
            icon.color = new Color(0, 0, 0, 0);
            icon.sprite = null;
            icon.enabled = false;
            removeButton.interactable = false;
        }

        public void OnRemoveButton()
        {
            Inventory.Instance.Remove(_item);
        }
    }
}