using Player;
using Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueStory
{
    [DisallowMultipleComponent]
    public class DialogueScreen : DestroySingleton<DialogueScreen>
    {
        public GameObject backupPrefab;
        public Text dialogueText;
        public Image characterSprite;
        public Canvas dialogueCanvas;

        /// <summary>
        /// Used when the dialogue screen is open but there is no text to be displayed yet.
        /// </summary>
        public const string DefaultEmptyText = "...";

        /// <summary>
        /// Speech text shown on the dialogue screen.
        /// </summary>
        public string Text
        {
            get => dialogueText.text;
            set => dialogueText.text = value;
        }

        /// <summary>
        /// Speaker shown on the dialogue screen.
        /// </summary>
        public string Speaker
        {
            set {
                if (value != null) SetCharacter(value);
            }
        }

        public bool CurrentlyTyping { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            InstantiateFieldsIfNull();

            Hide();
        }
        private void InstantiateFieldsIfNull()
        {
            GameObject go;
            if (dialogueCanvas == null)
            {
                Debug.Log("DialogueScreen.dialogueCanvas is null...attempting to find objects");
                go = LocateDialogueCanvas();
                if (go == null)
                {
                    Debug.Log("Cannot locate canvas.");
                    if (backupPrefab != null)
                    {
                        Debug.Log("BackupPrefab not null, instantiating backup prefab...");
                        go = Instantiate(backupPrefab);
                    }
                }

                dialogueCanvas = go.GetComponent<Canvas>();
            }

            if (dialogueText == null)
            {
                dialogueText = dialogueCanvas.transform.Find("DialogueText").GetComponent<Text>();
                Debug.Log("DialogueScreen.dialogueText is null...tried to find it: " + dialogueText);
            }

            if (characterSprite == null)
            {
                characterSprite = dialogueCanvas.transform.Find("CharacterImage").GetComponent<Image>();
                Debug.Log("DialogueScreen.characterSprite is null...tried to find it: " + characterSprite);
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
        /// Opens the dialogue canvas by enabling it and playing animations.
        /// </summary>
        public void Show()
        {
            dialogueText.text = DefaultEmptyText;       // dialogueText is null when re-entering scene in build version, single player 
            if (dialogueCanvas == null) InstantiateFieldsIfNull(); // i don't wanna debug myra's singleton code or why this s--- is null
            dialogueCanvas.gameObject.SetActive(true);
            PlayerScript.Instance.interactionEnabled = false;
            Debug.Log("Opened dialogue screen...");
        }

        /// <summary>
        /// Closes the dialogue canvas by disabling it and stopping animations.
        /// </summary>
        public void Hide()
        {
            CurrentlyTyping = false;
            PlayerScript.Instance.interactionEnabled = true;

            dialogueCanvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// Stops typing the previous dialogue, and instead displays the given line immediately.
        /// Intended to be used to "skip to the end" of a long dialogue line.
        /// </summary>
        /// <param name="line">Dialogue text to show.</param>
        /// TODO: is never actually used
        public void FinishDialogueLine(string line)
        {
            StopAllCoroutines();
            CurrentlyTyping = false;
            dialogueText.text = line;
        }

        /// <summary>
        /// Changes the character sprite to the given character.
        /// </summary>
        /// <param name="character">Character whose sprite we want to display.</param>
        private void SetCharacter(string character)
        {
            /*
            switch (character)
            {
                case "AGENT_Y":
                    characterSprite.sprite = Resources.Load<Sprite>("Sprite_AgentY");
                    break;
                case "PROTEUS":
                    characterSprite.sprite = Resources.Load<Sprite>("Sprite_Proteus");
                    break;
                case "NOVA":
                    characterSprite.sprite = Resources.Load<Sprite>("Sprite_Nova");
                    break;
                case "JULIUS":
                    characterSprite.sprite = Resources.Load<Sprite>("Sprite_Julius");
                    break;
                case "MARCUS":
                    characterSprite.sprite = Resources.Load<Sprite>("Sprite_Marcus");
                    break;
                case "CALYPSO":
                    characterSprite.sprite = Resources.Load<Sprite>("Sprite_Calypso");
                    break;
                case "AGENT_X":
                    characterSprite.sprite = Resources.Load<Sprite>("Sprite_AgentX");
                    break;
                case "HQLEADER":
                    characterSprite.sprite = Resources.Load<Sprite>("Sprite_HQLeader");
                    break;
            }
            */
        }
    }
}