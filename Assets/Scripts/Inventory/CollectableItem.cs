using DefaultNamespace;

namespace ItemInventory
{
    public class CollectableItem : Interactable
    {
        public Item item;
        public bool destroyOnPickup = true;
        private bool _pickedUp;

        public override void Interact(object src, params object[] args)
        {
            if (_pickedUp) return;
            base.Interact(src, args);
            Inventory.Instance.Add(item);
            if (destroyOnPickup)
                Destroy(gameObject);
            _pickedUp = true;
        }
    }
}