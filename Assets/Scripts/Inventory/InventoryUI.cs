using UnityEngine;
using Util;

namespace ItemInventory
{
    /// <summary>
    ///     Script for the inventory's UI.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        /// <summary>
        ///     Parent transform of all inventory slots.
        /// </summary>
        public Transform itemsParent;

        /// <summary>
        ///     Current Inventory UI. Note: NOT THE PREFAB.
        /// </summary>
        [Tooltip("Current inventory UI.")] public GameObject inventoryUI;

        private Inventory _inventory;
        private InventorySlot[] _slots;

        /// <summary>
        ///     On awake, find singleton inventory instance and disable the UI.
        /// </summary>
        private void Awake()
        {
            _inventory = Inventory.Instance;
            _inventory.onItemChanged.AddListener(UpdateUI);
            inventoryUI.SetActive(false);
            _slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        }

        /// <summary>
        ///     Checks every frame as to whether or not the inventory should be set active.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(SettingsManager.Instance.inventoryKey)) inventoryUI.SetActive(!inventoryUI.activeSelf);
        }

        /// <summary>
        ///     Updates the UI.
        /// </summary>
        public void UpdateUI()
        {
            for (int i = 0; i < _slots.Length; i++)
                if (i < _inventory.items.Count) _slots[i].AddItem(_inventory.items[i]);
                else _slots[i].ClearSlot();
        }
    }
}