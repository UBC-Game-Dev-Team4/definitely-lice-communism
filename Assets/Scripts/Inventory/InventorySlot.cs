using TMPro;
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
        [Tooltip("Name of item.")] public TextMeshProUGUI text;
        [Tooltip("Displayed icon.")] public Image icon;

        [Tooltip("Button to remove item.")] public Button removeButton;

        [Tooltip("Inventory highlight")] public Transform highlight;

        private Item _item;

        /// <summary>
        /// Gets/sets whether the inventory slot is highlighted
        /// </summary>
        public bool Highlighted
        {
            get => highlight.gameObject.activeInHierarchy;
            set => highlight.gameObject.SetActive(value);
        }

        /// <summary>
        ///     Adds the item to the slot.
        /// </summary>
        /// <param name="newItem"></param>
        public void AddItem(Item newItem)
        {
            _item = newItem;
            icon.color = new Color(255, 255, 255, 255);
            icon.sprite = _item.icon;
            text.text = _item.name;
            text.enabled = true;
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
            text.text = "";
            text.enabled = false;
            icon.enabled = false;
            removeButton.interactable = false;
        }

        public void OnRemoveButton()
        {
            Inventory.Instance.Remove(_item);
        }
    }
}