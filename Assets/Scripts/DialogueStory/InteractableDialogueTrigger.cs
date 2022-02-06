using DefaultNamespace;
using Player;
using UnityEngine;

namespace DialogueStory
{
    /// <summary>
    /// Enters dialogue when interacted with.
    /// </summary>
    [DisallowMultipleComponent]
    public class InteractableDialogueTrigger : Interactable
    {
        [Tooltip("Whether movement should be stopped")]
        public bool stopMovement;
        [Tooltip("Nullable string knot")]
        public string knot;
        
        /// <inheritdoc cref="Interactable"/>
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            if (stopMovement)
            {
                PlayerScript.Instance.movementEnabled = false;
            }
            BranchingStoryController.Instance.TryOpenDialogue(string.IsNullOrWhiteSpace(knot) ? BranchingStoryController.Instance.interrogationKnotTextCheck : knot);
        }
    }
}