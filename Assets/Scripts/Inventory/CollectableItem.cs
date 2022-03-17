using DefaultNamespace;
using UnityEngine;

namespace ItemInventory
{
    public class CollectableItem : Interactable
    {
        public Item item;
        public bool destroyOnPickup = true;
        public bool hasHighlight;
        public Sprite spriteOnHighlight;
        private bool _pickedUp;
        private SpriteRenderer _spriteRenderer;
        private Sprite _prevSprite;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _prevSprite = _spriteRenderer?.sprite;
        }

        public override void Highlight()
        {
            base.Highlight();
            if (!hasHighlight) return;
            if (_spriteRenderer != null && spriteOnHighlight != null)
            {
                _spriteRenderer.sprite = spriteOnHighlight;
            }
        }

        public override void DeHighlight()
        {
            base.DeHighlight();
            if (!hasHighlight) return;
            if (_spriteRenderer != null && _prevSprite != null)
            {
                _spriteRenderer.sprite = _prevSprite;
            }
        }

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