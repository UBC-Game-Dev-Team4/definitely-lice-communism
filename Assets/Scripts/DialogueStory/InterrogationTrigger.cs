using DefaultNamespace;
using UnityEngine;

namespace DialogueStory
{
    /// <summary>
    /// Enters the interrogation when interacted with.
    /// </summary>
    [DisallowMultipleComponent]
    public class InterrogationTrigger : Interactable
    {
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            // No dialogue comment should be associated with this trigger

            BranchingStoryController.Instance.TryInterrogate();
        }
    }
}