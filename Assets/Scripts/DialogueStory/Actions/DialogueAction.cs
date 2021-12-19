using UnityEngine;
using UnityEngine.Events;

namespace DialogueStory.Actions
{
    /// <summary>
    /// A dialogue action to execute
    /// </summary>
    [System.Serializable]
    public class DialogueAction
    {
        [System.Serializable] public class DialogueEvent : UnityEvent<DialogueActionData> { }

        [SerializeField] private DialogueActionId id;
        public string description;
        public DialogueActionId Id => id;

        public DialogueEvent action = new DialogueEvent();

        private DialogueAction()
        {
            if (action == null)
                action = new DialogueEvent();
        }

        public void Run(DialogueActionData data)
        {
            action.Invoke(data);
        }
    }
}