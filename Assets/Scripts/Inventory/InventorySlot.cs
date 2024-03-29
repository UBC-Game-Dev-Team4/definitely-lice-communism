using System;
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
        [Tooltip("Displayed name of item.")] public TextMeshProUGUI nameDisplay;
        [Tooltip("Displayed description of item.")] public TextMeshProUGUI descriptionDisplay;
        [Tooltip("Displayed icon.")] public Image icon;

        [Tooltip("Inventory highlight")] public Transform highlight;

        private Item _item;

        /// <summary>
        /// On awake clear slot just in case
        /// </summary>
        private void Awake()
        {
            if (_item == null)
                ClearSlot();
        }

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
            nameDisplay.text = _item.name;
            nameDisplay.enabled = true;
            descriptionDisplay.text = _item.description;
            descriptionDisplay.enabled = true;
            icon.enabled = true;
        }

        /// <summary>
        ///     Clears the icon from the slot and the item.
        /// </summary>
        public void ClearSlot()
        {
            _item = null;
            icon.color = new Color(0, 0, 0, 0);
            icon.sprite = null;
            nameDisplay.text = "";
            nameDisplay.enabled = false;
            descriptionDisplay.text = "";
            descriptionDisplay.enabled = false;
            icon.enabled = false;
        }
    }
}