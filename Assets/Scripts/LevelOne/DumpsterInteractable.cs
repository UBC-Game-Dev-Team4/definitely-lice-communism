using System;
using DefaultNamespace;
using ItemInventory;

namespace LevelOne
{
    /// <summary>
    ///     A dumpster that is interactable. Gives player an item upon interaction.
    /// </summary>
    public class DumpsterInteractable
    {
        public bool isInteractable;
        public Item item;
        private bool _pickedUp;
        public override void Interact(object src, params object[] args)
        {
            if (_pickedUp) return;
            if (!isInteractable) return;
            base.Interact(src, args);
            Inventory.Instance.Add(item);
            Destroy(gameObject);
            _pickedUp = true;
        }
    }
}