using DialogueStory;
using ItemInventory;
using Player;
using UnityEngine;

namespace LevelOne.Cat
{
    /// <summary>
    /// Item to define custom logic for an inventory item
    /// </summary>
    /// <seealso cref="ItemInventory.Item"/>
    [CreateAssetMenu(fileName = "Cat Food Item", menuName = "Scene One/Cat Food Item")]
    public class CatFoodItem : Item
    {
        /// <summary>
        /// Delegate defining a method to be called on place
        /// </summary>
        public delegate void OnPlaceDelegate();

        /// <summary>
        /// Event triggered on cat food item placed
        /// </summary>
        public static event OnPlaceDelegate OnPlaceEvent;

        /// <summary>
        /// Whether the cat food is placed or not
        /// </summary>
        public static bool IsPlaced
        {
            get;
            private set;
        }

        /// <summary>
        /// Transform giving the actual item in the scene
        /// </summary>
        public static Transform PlacedLocation
        {
            get;
            private set;
        }
        [Tooltip("Prefab of the cat food object to be created")]
        public GameObject catFoodPrefab;

        [Tooltip("Knot of dialogue to play")]
        public string dialogueEvent;
        /// <inheritdoc cref="ItemInventory.Item.Use"/>
        public override void Use()
        {
            IsPlaced = true;
            GameObject go = Instantiate(catFoodPrefab, PlayerScript.Instance.transform.position, Quaternion.identity);
            PlacedLocation = go != null ? go.transform : null;
            Inventory.Instance.Remove(this);
            OnPlaceEvent?.Invoke();
            DialogueTrigger.TriggerDialogue(dialogueEvent, true);
        }

        /// <summary>
        /// On destroy, remove the static delegates/events
        /// </summary>
        private void OnDestroy()
        {
            if (OnPlaceEvent == null) return;
            foreach (OnPlaceDelegate d in OnPlaceEvent.GetInvocationList())
            {
                OnPlaceEvent -= d;
            }
        }
    }
}