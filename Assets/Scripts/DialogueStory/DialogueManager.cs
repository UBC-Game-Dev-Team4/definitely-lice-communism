using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace DialogueStory
{
    public class DialogueManager : Singleton<DialogueManager>, IStoryPlayer
    {
        [Tooltip("Array of buttons for choices, in the same order as the hierarchy.")]
        public Button[] buttons;

        public StoryStates State => StoryStates.InDialogue;

        protected override void Awake()
        {
            base.Awake();

            InitializeFieldsIfNull();

            InitializeChoiceButtons();
        }

        private void InitializeFieldsIfNull()
        {
            if (buttons != null) return;
            Debug.Log("DialogueManager.buttons is null...attempting to find objects");
            GameObject go = LocateDialogueCanvas();

            Transform userInput = go.transform.Find("UserInput");
            buttons = new Button[4];
            for (int i = 0; i < 4; i++) {
                buttons[i] = userInput.Find("Choice" + (i + 1)).GetComponent<Button>();
                Debug.Log("Tried to locate button in dialogue canvas: " + buttons[i]);
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
                    StartCoroutine(AdvanceDialogueAndShow());
                });
                b.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Coroutine that advances the dialogue line by line, skipping empty lines, and types out each line.
        /// After the last line of dialogue is shown, shows the choices upon a key press.
        /// <para>If it encounters an end tag or input tag, however, the coroutine will stop and control will be given to the
        /// respective behaviours (closing the dialogue/showing the input UI).</para>
        /// </summary>
        private IEnumerator AdvanceDialogueAndShow()
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

                BranchingStoryController.Instance.RunPuzzleEvents(this);

                // Type out the line only if it is not empty.
                string line = BranchingStoryController.Instance.CurrentLine;
                if (!string.IsNullOrWhiteSpace(line))
                {
                    yield return StartCoroutine(PlayLineAndWaitForNextKey(line));
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
                    Debug.Log("Choice " + (i + 1) + ":" + choices[i].text);
                    buttons[i].GetComponentInChildren<Text>().text = choices[i].text;
                    buttons[i].gameObject.SetActive(true);
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Opens the dialogue screen and begins the dialogue
        /// </summary>
        public void Open()
        {
            DialogueScreen.Instance.Show();
            StartCoroutine(AdvanceDialogueAndShow());
        }

        /// <summary>
        /// Closes the dialogue, and releases control of the game story.
        /// </summary>
        public void Close()
        {
            DialogueScreen.Instance.Hide();

            // Game story will handle releasing the input manager here.
            BranchingStoryController.Instance.DialogueCloseEvent.Invoke();
        }
    }
}