using Player;
using UnityEngine;

namespace DialogueStory
{
    /// <summary>
    /// Enters dialogue when method is called.
    /// </summary>
    [DisallowMultipleComponent]
    public class DialogueTrigger : MonoBehaviour
    {
        [Tooltip("Whether movement should be stopped")]
        public bool stopMovement;
        [Tooltip("Knot/choice to jump to")]
        public string knot;

        /// <summary>
        /// Play a dialogue from a specific knot/choice
        /// </summary>
        public virtual void PlayDialogue()
        {
            TriggerDialogue(knot, stopMovement);
        }

        /// <summary>
        /// Trigger dialogue with given starting knot/dialogue and whether movement is stopped
        /// </summary>
        /// <param name="dialogue">Starting knot/dialogue option</param>
        /// <param name="stopMovement">Whether movement should be stopped</param>
        /// <returns>True if dialogue is played (i.e. dialogue knot is not null/empty), false otherwise</returns>
        public static bool TriggerDialogue(string dialogue, bool stopMovement = false)
        {
            if (string.IsNullOrWhiteSpace(dialogue)) return false;
            if (stopMovement) PlayerScript.Instance.movementEnabled = false;
            BranchingStoryController.Instance.TryOpenDialogue(dialogue);
            return true;
        }
    }
}