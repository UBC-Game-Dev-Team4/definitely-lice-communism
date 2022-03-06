using System;
using System.Collections.Generic;
using System.Linq;
using Singleton;
using UnityEngine;

namespace DialogueStory.Actions
{
    /// <summary>
    /// Manages the lookup of IDs to Dialogue Actions
    /// </summary>
    /// <remarks>
    /// Copied over from old code
    /// </remarks>
    [AddComponentMenu("Dialogue/DialogueActionManager")]
    public sealed class DialogueActionManager : DestroySingleton<DialogueActionManager>
    {
        public DialogueAction[] actions = Array.Empty<DialogueAction>();
        private readonly List<DialogueAction> _listOfActions = new List<DialogueAction>();

        private Dictionary<DialogueActionId, DialogueAction> _actions =
            new Dictionary<DialogueActionId, DialogueAction>();

        private void OnValidate()
        {
            _actions = actions.ToDictionary(action => action.Id);
            _listOfActions.AddRange(_listOfActions);
        }

        public void AddAction(DialogueAction action)
        {
            _listOfActions.Add(action);
            actions = _listOfActions.ToArray();
            _actions.Add(action.Id,action);
        }

        public bool RemoveAction(DialogueAction action)
        {
            _listOfActions.Remove(action);
            actions = _listOfActions.ToArray();
            return _actions.Remove(action.Id);
        }

        public void RunDialogueAction(DialogueActionId id, DialogueActionData data)
        {
            if (_actions.TryGetValue(id, out DialogueAction action))
            {
                action.Run(data);
            }
            else
            {
                Debug.LogWarning("Failed to find puzzle action with id: " + id);
            }
        }

    }
}
