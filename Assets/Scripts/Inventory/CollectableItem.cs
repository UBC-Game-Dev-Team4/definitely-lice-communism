using DefaultNamespace;

namespace ItemInventory
{
    public class CollectableItem : Interactable
    {
        public Item item;
        private bool _pickedUp;

        public override void Interact(object src, params object[] args)
        {
            if (_pickedUp) return;
            base.Interact(src, args);
            Inventory.Instance.Add(item);
            Destroy(gameObject);
            _pickedUp = true;
        }
    }
}