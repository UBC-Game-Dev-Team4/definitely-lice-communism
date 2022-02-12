using System.Collections;
using UnityEngine;

namespace DialogueStory
{
    /// <summary>
    /// Script to display dialogue on the first frame of a scene
    /// </summary>
    [DisallowMultipleComponent]
    public class OnFirstFrameDialogueTrigger : DialogueTrigger
    {
        /// <summary>
        /// On awake start the coroutine to display text
        /// </summary>
        private void Awake()
        {
            StartCoroutine(nameof(OnFirstFrameCoroutine));
        }
        
        /// <summary>
        /// Coroutine that plays the dialogue
        /// </summary>
        /// <returns>Coroutine - return value should not be used internally</returns>
        private IEnumerator OnFirstFrameCoroutine()
        {
            yield return null; // wait for first frame
            base.PlayDialogue();
            StopCoroutine(nameof(OnFirstFrameCoroutine));
        }
    }
}