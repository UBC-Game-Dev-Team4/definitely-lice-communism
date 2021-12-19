using UnityEngine;

namespace DialogueStory.Actions
{
    /// <summary>
    /// Keyboard trigger for target actions
    /// </summary>
    [AddComponentMenu("Dialogue/DialogueActionTrigger")]
    public class DialogueActionTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueActionId[] targetPuzzleActions;
        [SerializeField] private DialogueActionData data;
        public DialogueActionId[] TargetPuzzleActions { get => targetPuzzleActions; }

        [Header("Testing")]
        [SerializeField] private KeyCode keyToTrigger;

        private void Update()
        {
            if (Input.GetKeyDown(keyToTrigger)) Trigger();
        }
    
        /// <summary>
        /// Trigger all referenced PuzzleActions.
        /// </summary>
        public void Trigger()
        {
            if (targetPuzzleActions.Length == 0)
            {
                Debug.LogWarning( nameof(DialogueActionTrigger)+ "does not have any "+nameof(DialogueAction)+" assigned.");
            }

            foreach (DialogueActionId id in targetPuzzleActions)
            {
                DialogueActionManager.Instance.RunDialogueAction(id,data);
            }
        }
    }
}
