using DialogueStory;
using UnityEngine;

namespace LevelOne.Cat
{
    /// <summary>
    /// Trigger to display dialogue upon player seeing the cat
    /// </summary>
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