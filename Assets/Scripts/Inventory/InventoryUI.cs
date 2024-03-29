using UnityEngine;
using Util;

namespace ItemInventory
{
    /// <summary>
    ///     Script for the inventory's UI.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Tooltip("Parent transform of all inventory slots")]
        public Transform itemsParent;

        /// <remarks>
        /// NOT THE PREFAB
        /// </remarks>
        [Tooltip("Current inventory UI.")] public GameObject inventoryUI;

        [Tooltip("Audio source on switch")]
        public AudioSource audioSourceOnSwitch;

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

            if (!inventoryUI.activeInHierarchy) return;
            if (Input.GetKeyDown(SettingsManager.Instance.inventoryUp))
            {
                int oldIndex = _inventory.indexOfSelection;
                if (oldIndex >= 0 && oldIndex < _slots.Length)
                    _slots[oldIndex].Highlighted = false;
                int newIndex = oldIndex - 1;
                if (newIndex < 0) newIndex = _slots.Length - 1;
                _inventory.indexOfSelection = newIndex;
                if (newIndex >= 0 && newIndex < _slots.Length)
                    _slots[newIndex].Highlighted = true;
                audioSourceOnSwitch.Play();
            }
            if (Input.GetKeyDown(SettingsManager.Instance.inventoryDown))
            {
                int oldIndex = _inventory.indexOfSelection;
                if (oldIndex >= 0 && oldIndex < _slots.Length)
                    _slots[oldIndex].Highlighted = false;
                int newIndex = oldIndex + 1;
                if (newIndex >= _slots.Length) newIndex = 0;
                _inventory.indexOfSelection = newIndex;
                if (newIndex >= 0 && newIndex < _slots.Length)
                    _slots[newIndex].Highlighted = true;
                audioSourceOnSwitch.Play();
            }
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