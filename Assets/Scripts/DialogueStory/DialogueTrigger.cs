using DefaultNamespace;
using UnityEngine;

namespace DialogueStory
{
    /// <summary>
    /// Enters dialogue when interacted with.
    /// </summary>
    [DisallowMultipleComponent]
    public class DialogueTrigger : Interactable
    {
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            BranchingStoryController.Instance.TryOpenDialogue();
        }
    }
}