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
            if (string.IsNullOrEmpty(knot)) return;
            if (stopMovement)
            {
                PlayerScript.Instance.movementEnabled = false;
            }
            BranchingStoryController.Instance.TryOpenDialogue(knot);
        }
    }
}