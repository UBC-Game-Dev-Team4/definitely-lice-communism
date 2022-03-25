using DialogueStory;
using ItemInventory;
using Player;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Item to define custom logic for the mallet item
    /// </summary>
    /// <seealso cref="ItemInventory.Item"/>
    [CreateAssetMenu(fileName = "Plate Item", menuName = "Scene One/Plate Item")]
    public class PlateItem : Item
    {
        [Tooltip("Prefab of plate")]
        public GameObject platePrefab;
        [Tooltip("Offset of placed prefab")]
        public Vector3 positionOffset = Vector3.zero;
        [Tooltip("Knot of dialogue to play")]
        public string dialogueEvent = "OnPlateDrop";
        
        /// <inheritdoc cref="ItemInventory.Item.Use"/>
        public override void Use()
        {
            Vector3 playerPos = PlayerScript.Instance.transform.position;
            Instantiate(platePrefab, playerPos + positionOffset, Quaternion.identity);
            Inventory.Instance.Remove(this);
            DialogueTrigger.TriggerDialogue(dialogueEvent, true);
            ChefScript.InstanceIfPresent?.MoveFromKitchen();
        }
    }
}