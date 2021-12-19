using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace DialogueStory
{
    // TODO: Only handles typing in 1 word at a time
    public class InterrogationManager : Singleton<InterrogationManager>, IStoryPlayer
    {
        [Tooltip("Array of buttons for choices, in the same order as the hierarchy.")]
        public Button[] buttons;
        [Tooltip("Parent of input text field and button (should have a Horizontal Layout Group).")]
        public GameObject inputParent;
        [Tooltip("Input field for taking user input in the interrogation.")]
        public InputField input;

        public StoryStates State => StoryStates.Interrogation;

        protected override void Awake()
        {
            base.Awake();

            InitializeFieldsIfNull();

            inputParent.GetComponentInChildren<Button>().onClick.AddListener(() => ProcessInput());
            inputParent.gameObject.SetActive(false);

            InitializeChoiceButtons();
        }

        private void InitializeFieldsIfNull()
        {
            GameObject go = null;
            if (buttons == null)
            {
                Debug.Log("InterrogationManager.buttons is null...attempting to find objects");
                go = LocateDialogueCanvas();

                Transform userInput = go.transform.Find("UserInput");
                buttons = new Button[4];
                for (int i = 0; i < 4; i++) {
                    buttons[i] = userInput.Find("Choice" + (i + 1)).GetComponent<Button>();
                    Debug.Log("Tried to locate button in dialogue canvas: " + buttons[i]);
                }
            }

            if (inputParent == null)
            {
                Debug.Log("InterrogationManager.inputParent is null...attempting to find objects");
                if (go == null) {
                    go = LocateDialogueCanvas();
                }
                inputParent = go.transform.Find("UserInput").Find("InputParent").gameObject;
                Debug.Log("Tried to locate inputParent in dialogue canvas: " + inputParent);
            }

            if (input == null)
            {
                Debug.Log("InterrogationManager.input is null...attempting to find objects");

                input = inputParent.GetComponentInChildren<InputField>();
                Debug.Log("Tried to locate input in inputParent: " + input);
            }
        }

        /// <summary>
        /// Locates dialogue canvas, even if it's inactive.
        /// </summary>
        /// <returns>DialogueCanvas, if one exists</returns>
        private static GameObject LocateDialogueCanvas()
        {
            GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject go in allGameObjects)
            {
                if (go.name.Equals("DialogueCanvas")) return go;
            }
            Debug.LogError("DialogueCanvas not in scene. Returning null.");
            return null;
        }

        /// <summary>
        /// Initializes 3 buttons for choices and disables their gameObjects for now.
        /// </summary>
        private void InitializeChoiceButtons()
        {
            Debug.Log("Initializing the choice buttons....");

            foreach (Button b in buttons)
            {
                b.onClick.AddListener(() =>
                {
                    Debug.Log("Clicked a choice button");
                    BranchingStoryController.Instance.TryMoveChoice(this, b.transform.GetSiblingIndex());
                    //PlayNextLine();
                    StartCoroutine(AdvanceInterrogationAndShow());
                });
                b.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Called when input is active and "enter" button is pressed.
        /// Processes input from the text field, making the choice in the ink story that corresponds to
        /// the user's inputted text.
        /// </summary>
        private void ProcessInput()
        {
            string entered = input.text;
            for (int i = 2; i < BranchingStoryController.Instance.CurrentChoices.Count; i++)
            {
                string choice = BranchingStoryController.Instance.CurrentChoices[i].text;
                if (!string.Equals(choice, entered, StringComparison.OrdinalIgnoreCase)) continue;
                BranchingStoryController.Instance.TryMoveChoice(this, i);
                StartCoroutine(AdvanceInterrogationAndShow());
                return;
            }

            // Pick choice "else", which should be at index 1 in the ink story
            BranchingStoryController.Instance.TryMoveChoice(this, 1);
            StartCoroutine(AdvanceInterrogationAndShow());
        }

        /// <summary>
        /// Activates the input field, input submit button, and shows a single choice to go back.
        /// </summary>
        private void ShowInput()
        {
            input.text = BranchingStoryController.Instance.CurrentLine;
            inputParent.gameObject.SetActive(true);

            ShowInputChoices();
        }

        /// <summary>
        /// Deactivates the input field and input submit button.
        /// </summary>
        private void HideInput()
        {
            inputParent.gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the choices corresponding to the input-accepting state (i.e. a single choice to exit the user input screen)
        /// </summary>
        private void ShowInputChoices()
        {
            // Choice to exit should be at index 0 in the ink story
            buttons[0].GetComponentInChildren<Text>().text = "Back";
            buttons[0].gameObject.SetActive(true);

            for (int i = 1; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Coroutine that advances the interrogation line by line, skipping empty lines, and types out each line.
        /// After the last line of dialogue is shown, shows the choices upon a key press.
        /// <para>If it encounters an end tag or input tag, however, the coroutine will stop and control will be given to the
        /// respective behaviours (closing the interrogation/showing the input UI).</para>
        /// </summary>
        private IEnumerator AdvanceInterrogationAndShow()
        {
            HideChoices();

            while (BranchingStoryController.Instance.TryMoveNext(this))
            {
                if (BranchingStoryController.Instance.CheckForEnd(this))
                {
                    // Stop this coroutine and hide the dialogue screen - ensures this coroutine won't be called again
                    Close();
                    yield break;
                }

                if (BranchingStoryController.Instance.CheckForInput())
                {
                    // Show input-related UI and wait for input submit button, which will restart this coroutine.
                    ShowInput();
                    yield break;
                    // TODO
                }
                else
                {
                    HideInput();
                    BranchingStoryController.Instance.RunPuzzleEvents(this);

                    // Type out the line only if it is not empty.
                    string line = BranchingStoryController.Instance.CurrentLine;
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        yield return StartCoroutine(PlayLineAndWaitForNextKey(line));
                    }
                }
            }

            // Wait for button press, which will restart this coroutine.
            ShowChoices();
        }

        /// <summary>
        /// Coroutine that types out a line character by character.
        /// If typing is interrupted by pressing the next key, skips to the end of the line.
        /// When the line is finished, waits for user to press the next key again before ending coroutine.
        /// </summary>
        /// <param name="line">Raw line of text to parse and display.</param>
        private IEnumerator PlayLineAndWaitForNextKey(string line)
        {
            DialogueUtils.ExtractSpokenLine(line, out string text, out string speaker);

            DialogueScreen.Instance.Speaker = speaker;
            DialogueScreen.Instance.Text = "";

            foreach (char letter in text)
            {
                yield return null;
                DialogueScreen.Instance.Text += letter;

                // If next key pressed early, skip to the end of the line.
                if (!Input.GetKeyDown(SettingsManager.Instance.nextDialogueKey)) continue;
                DialogueScreen.Instance.Text = text;
                yield return null;
                break;
            }

            // Wait for next key press to advance dialogue.
            while (!Input.GetKeyDown(SettingsManager.Instance.nextDialogueKey))
            {
                yield return null;
            }
        }

        /// <summary>
        /// Hides all choices.
        /// </summary>
        private void HideChoices()
        {
            foreach (Button t in buttons) t.gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the current choices at this point in the story.
        /// </summary>
        private void ShowChoices()
        {
            List<Choice> choices = BranchingStoryController.Instance.CurrentChoices;

            Debug.Log("Showing " + choices.Count + " choices.");

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < choices.Count)
                {
                    buttons[i].gameObject.SetActive(true);

                    Debug.Log("Choice " + (i + 1) + ":" + choices[i].text);

                    buttons[i].GetComponentInChildren<Text>().text = choices[i].text;
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Opens the dialogue screen and begins the interrogation.
        /// </summary>
        public void Open()
        {
            DialogueScreen.Instance.Show();
            StartCoroutine(AdvanceInterrogationAndShow());
        }

        /// <summary>
        /// Closes the interrogation, and releases control of the game story.
        /// </summary>
        public void Close()
        {
            DialogueScreen.Instance.Hide();

            // Game story will handle releasing the input manager here.
            BranchingStoryController.Instance.InterrogationCloseEvent.Invoke();
        }
    }
}