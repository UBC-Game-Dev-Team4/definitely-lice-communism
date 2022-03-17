using DefaultNamespace;
using DialogueStory;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Script to be attached to a door to play dialogue upon locked interaction
    /// </summary>
    [DisallowMultipleComponent]
    public class DoorInteractDialogueTrigger : DialogueTrigger
    {
        [Tooltip("Door to count interactions to")]
        public DoorScript door;

        [Tooltip("Knot/dialogue option to switch to upon multiple interaction")]
        public string knotOnMany;
        [Tooltip("Cutoff for switching dialogue")]
        public int cutoffForMany = 3;
        private int _prevCount = 0;

        /// <summary>
        /// On interact increment counter and display dialogue
        /// </summary>
        public void OnInteract()
        {
            if (door.LockedInteractCount == _prevCount) return;
            _prevCount = door.LockedInteractCount;
            if (_prevCount >= cutoffForMany)
            {
                knot = knotOnMany;
            }
            PlayDialogue();
        }
    }
}