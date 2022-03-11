using System;
using DefaultNamespace;
using ItemInventory;

namespace LevelOne
{
    public class CatInteractable : Interactable
    {
        public bool isInteractable;
        public Item item;
        private CatAIScript _cat;
        private bool _pickedUp;

        private void Awake()
        {
            _cat = GetComponentInParent<CatAIScript>();
        }

        public override void Interact(object src, params object[] args)
        {
            if (_pickedUp) return;
            if (!isInteractable) return;
            base.Interact(src, args);
            Inventory.Instance.Add(item);
            Destroy(gameObject);
            _pickedUp = true;
            _cat.OnKeyTaken();
        }
    }
}