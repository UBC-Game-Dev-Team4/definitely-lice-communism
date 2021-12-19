using System;
using System.Collections.Generic;
using DialogueStory.Actions;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueStory
{
    /// <summary>
    /// Controls progress through the branching story.
    /// Whereas BranchingStory is the raw Ink content, the BranchingStoryController wraps the BranchingStory
    /// and manipulates it in the context of the game.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BranchingStory))]
    public sealed class BranchingStoryController : Singleton<BranchingStoryController>
    {
        private BranchingStory _inkStory;

        /// <summary>
        /// Current location of the story.
        /// </summary>
        public StoryStates State { get; private set; }

        /// <summary>
        /// Current line in the ink story, having had no processing performed on it.
        /// </summary>
        public string CurrentLine => _inkStory.CurrentLine;

        /// <summary>
        /// List of choices at the current point in the story.
        /// </summary>
        public List<Choice> CurrentChoices => _inkStory.CurrentChoices;

        /// <summary>
        /// Tags attached to the current line in the story.
        /// </summary>
        public Dictionary<string, string> CurrentTags => _inkStory.CurrentTags;

        /// <summary>
        /// Event invoked immediately after the interrogation ends. Should not be modified by non story-related scripts.
        /// </summary>
        public UnityEvent InterrogationCloseEvent { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            State = StoryStates.Idle;

            if (InterrogationCloseEvent == null) InterrogationCloseEvent = new UnityEvent();
            InterrogationCloseEvent.AddListener(TryLeaveInterrogation);
            
        }

        private void Start()
        {
            _inkStory = GetComponent<BranchingStory>();
            _inkStory.ContinueUntilChoice();
        }

        /// <summary>
        /// Opens the interrogation if the game story is currently idle, otherwise does nothing. Enters UI mode.
        /// </summary>
        public void TryInterrogate()
        {
            if (State != StoryStates.Idle) return;
            // Move to the interrogation knot, but don't continue
            _inkStory.MakeChoice(1);

            EnterStoryMode();
            State = StoryStates.Interrogation;

            InterrogationManager.Instance.Open();
        }

        /// <summary>
        /// After the interrogation UI has closed itself, resets internal story state to idle and exits UI mode.
        /// </summary>
        private void TryLeaveInterrogation()
        {
            if (State != StoryStates.Interrogation) return;
            ExitStoryMode();
            _inkStory.ContinueUntilChoice();
        }

        private void EnterStoryMode()
        {
            Debug.Log(State);
            //CustomInputManager.CurrentMode = InputMode.UIInput;
        }

        private void ExitStoryMode()
        {
            State = StoryStates.Idle;
            //CustomInputManager.CurrentMode = InputMode.GameMovement;
        }

        /// <summary>
        /// If the story is able to continue, moves it to the next line.
        /// <para>Note: this method is only meant to be called by the IStoryPlayer currently in control of the story.
        /// Trying to guess the argument p will most likely mess up the internal state of the story.</para>
        /// </summary>
        /// <returns>True if story continued, false if it couldn't continue.</returns>
        public bool TryMoveNext(IStoryPlayer p)
        {
            if (p != null && State == p.State)
            {
                return _inkStory.Continue();
            }
            return false;
        }

        /// <summary>
        /// If it is able, moves the story along the choice at the given index.
        /// Does not load the result of the choice; to do so, call <see cref="TryMoveNext(IStoryPlayer)">TryMoveNext</see>.
        /// <para>Note: this method is only meant to be called from the IStoryPlayer currently in control of the story.
        /// Trying to guess the argument p will most likely mess up the internal state of the story.</para>
        /// </summary>
        /// <param name="index">Zero-based index of the choice.</param>
        public void TryMoveChoice(IStoryPlayer p, int index)
        {
            if (State == p.State)
            {
                _inkStory.MakeChoice(index);
            }
        }

        #region Utility Functions

        /// <summary>
        /// Utility function that runs all puzzle events associated with the current line;
        /// meant to be called by IStoryPlayers when running the story.
        /// </summary>
        /// <param name="p">StoryPlayer from which this method is being called.</param>
        public void RunPuzzleEvents(IStoryPlayer p)
        {
            if (p.State != State) return;
            if (!CurrentTags.TryGetValue("name", out string tagName)) return;
            DialogueActionId id = new DialogueActionId()
            {
                name = tagName
            };

            CurrentTags.TryGetValue("data", out string tagData);
            DialogueActionData dialogueActionData = new DialogueActionData
            {
                data = tagData == null ? 0 : int.Parse(tagData)
            };

            DialogueActionManager.Instance.RunDialogueAction(id, dialogueActionData);
        }

        /// <summary>
        /// Checks whether the current line is meant to take input from the user.
        /// </summary>
        /// <returns>True if the current line is user input, false otherwise.</returns>
        public bool CheckForInput()
        {
            return CurrentTags.ContainsKey("input");
        }

        /// <summary>
        /// Utility function that checks whether the current tags signal the end of the story;
        /// meant to be called by IStoryPlayers when running the story.
        /// </summary>
        /// <param name="p">StoryPlayer from which this method is being called.</param>
        /// <returns>True if the end tag was detected, false otherwise.</returns>
        public bool CheckForEnd(IStoryPlayer p)
        {
            if (p.State != State || !CurrentTags.ContainsKey("end")) return false;
            Debug.Log("End tag detected!");
            return true;
        }

        #endregion

        #region Change Story Variables
        /// <summary>
        /// Sets the value of the given story variable.
        /// </summary>
        /// <param name="varName">Name of story variable to set value of.</param>
        /// <param name="newValue">New value for the story variable.</param>
        public void SetStoryVariable(string varName, bool newValue) => _inkStory.SetStoryVariable(varName, newValue);

        /// <summary>
        /// Sets the value of the given story variable.
        /// </summary>
        /// <param name="varName">Name of story variable to set value of.</param>
        /// <param name="newValue">New value for the story variable.</param>
        public void SetStoryVariable(string varName, int newValue) => _inkStory.SetStoryVariable(varName, newValue);

        /// <summary>
        /// Sets the value of the given story variable.
        /// </summary>
        /// <param name="varName">Name of story variable to set value of.</param>
        /// <param name="newValue">New value for the story variable.</param>
        public void SetStoryVariable(string varName, string newValue) => _inkStory.SetStoryVariable(varName, newValue);

        /// <summary>
        /// Gets the value of the given story variable.
        /// </summary>
        /// <param name="varName">Name of the variable we are looking for.</param>
        /// <returns>The value.</returns>
        public object GetVariableValue(string varName) => _inkStory.GetVariableValue(varName);

        #endregion

        // TODO: Currently untested
        // TODO: Handle invalid input during the load
        // TODO: When restarting a level, we want to always start in the idle state.
        //       Must ensure that the story is in idle state when saving, hence that we load a story already in idle state,
        //       AND/OR that we always jump to the idle state when reloading the level.
        #region Save/Load Story

        /// <summary>
        /// Returns the current Ink story state encoded as a JSON string.
        /// The saved string can be used to load back to this state of the story later.
        /// </summary>
        /// <returns>A JSON string capturing the current story state.</returns>
        public string GetCurrentStoryState() => _inkStory.GetCurrentStoryState();

        /// <summary>
        /// Sets the story state to a saved state encoded as a JSON string.
        /// </summary>
        /// <param name="savedJson">The JSON representation of a story state, obtained by calling <see cref="GetCurrentStoryState"/></param>
        public void LoadStoryState(string savedJson) => _inkStory.LoadStoryState(savedJson);

        #endregion
    }
}