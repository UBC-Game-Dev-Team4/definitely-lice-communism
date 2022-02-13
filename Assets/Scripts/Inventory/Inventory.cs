using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ItemInventory
{
    /// <summary>
    ///     Script for having an inventory attached to a player.
    /// </summary>
    [DisallowMultipleComponent]
    public class Inventory : MonoBehaviour
    {
        
        #region Singleton Design Pattern

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        private static Inventory _privateStaticInstance;

        /// <summary>
        ///     Getter for the singleton.
        /// </summary>
        public static Inventory Instance
        {
            get
            {
                if (_privateStaticInstance == null) _privateStaticInstance = FindObjectOfType<Inventory>();
                if (_privateStaticInstance != null) return _privateStaticInstance;
                GameObject go = new GameObject("Inventory Manager");
                _privateStaticInstance = go.AddComponent<Inventory>();
                return _privateStaticInstance;
            }
        }

        #endregion
        
        [Tooltip("Runs on item added/removed.")]
        public UnityEvent onItemChanged = new UnityEvent();

        [Min(0)] [Tooltip("Maximum number of items in inventory.")]
        public int space = 20;

        [Tooltip("List of items in inventory.")]
        public List<Item> items = new List<Item>();

        internal int indexOfSelection = -1;

        /// <summary>
        ///     On awake, set this to private static instance
        /// </summary>
        private void Awake()
        {
            if (_privateStaticInstance != null && _privateStaticInstance != this)
                Debug.LogWarning("More than one instance of Inventory found!");
            else _privateStaticInstance = this;
        }

        /// <summary>
        ///     Adds an item to the inventory, if space allows.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Whether or not the item was successfully added</returns>
        public bool Add(Item item)
        {
            if (item.isDefaultItem) return true;
            if (items.Count >= space)
            {
                Debug.Log("NO SPACE IN INVENTORY");
                return false;
            }

            items.Add(item);
            onItemChanged.Invoke();
            return true;
        }

        /// <summary>
        ///     Removes the item from the inventory.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(Item item)
        {
            items.Remove(item);
            onItemChanged.Invoke();
        }

        /// <summary>
        /// Determines whether the item is currently being active/held in the inventory
        /// </summary>
        /// <param name="item">Item in question</param>
        /// <returns>Whether the item is active or not</returns>
        public bool HasActiveItem(Item item)
        {
            if (indexOfSelection < 0 || indexOfSelection >= items.Count) return false;
            if (item == null) return false;
            return items[indexOfSelection].name == item.name;
        }
    }
}