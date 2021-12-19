using UnityEngine;

namespace Util
{
    /// <summary>
    ///     Script to hold global settings objects
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        /// <summary>
        ///     Private single instance of <see cref="SettingsManager" />
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Singleton_pattern" />
        private static SettingsManager _settings;

        [Tooltip("Key to press to jump")] public KeyCode jumpKey = KeyCode.Space;

        [Tooltip("Key to open inventory")] public KeyCode inventoryKey = KeyCode.I;
        
        [Tooltip("Inventory to progress dialogue")]
        public KeyCode nextDialogueKey = KeyCode.F;

        [Tooltip("Key to interact with objects")]
        public KeyCode interactKey = KeyCode.E;

        /// <summary>
        ///     Accessor to singleton instance of <see cref="SettingsManager" />
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Singleton_pattern" />
        public static SettingsManager Instance
        {
            get
            {
                if (_settings == null) _settings = FindObjectOfType<SettingsManager>();
                if (_settings != null) return _settings;
                GameObject go = new GameObject("SettingsManager");
                _settings = go.AddComponent<SettingsManager>();
                return _settings;
            }
        }

        /// <summary>
        ///     Sets don't destroy on load and set singleton instance
        ///     Runs upon object being added/initialized, but before Start
        /// </summary>
        private void Awake()
        {
            _settings = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}