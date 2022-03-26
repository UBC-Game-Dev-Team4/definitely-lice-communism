using DefaultNamespace;
using ItemInventory;
using UnityEngine;

namespace LevelOne.Cat
{
    public class CatInteractable : Interactable
    {
        public bool isInteractable;
        public Item item;
        public AudioSource onTakeSound;
        public int priorityChangeWhenInteractable = 3;
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
            _cat.keyAmbient.StopPlaying();
            onTakeSound.Play();
            Destroy(gameObject);
            _pickedUp = true;
            _cat.OnKeyTaken();
        }
    }
}