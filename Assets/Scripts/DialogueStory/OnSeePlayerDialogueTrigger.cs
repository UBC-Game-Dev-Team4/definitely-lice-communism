using UnityEngine;

namespace DialogueStory
{
    /// <summary>
    /// One-time trigger to display dialogue upon player seeing the object
    /// </summary>
    /// <remarks>
    /// Object gets deleted, so do not attach anything else that is important.
    /// </remarks>
    [DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
    public class OnSeePlayerDialogueTrigger : DialogueTrigger
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayDialogue();
            Destroy(gameObject);
        }
    }
}