using DefaultNamespace;
using ItemInventory;

namespace LevelOne
{
    /// <summary>
    ///     Plate is an interactable item that can be collected into inventory and break.
    /// </summary>
    public class PlateInteractable : CollectableItem
    {
        public bool isInteractable;
        private bool _pickedUp;
        private bool _broken;

        public override void Interact(object src, params object[] args)
        {
            if (_pickedUp) return;
            if (!isInteractable) return;
            if (_broken) return;
            base.Interact(src, args);
            _broken = false;
        }
        
        /// <summary>
        /// breaks the plate, item is dropped and is no longer interactable
        /// </summary>
        public void Break()
        {
            _broken = true;
            _pickedUp = false;
            isInteractable = false;
        }
    }
}