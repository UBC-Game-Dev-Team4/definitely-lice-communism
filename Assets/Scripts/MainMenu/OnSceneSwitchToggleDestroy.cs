using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    /// <summary>
    /// Script that will disable DontDestroyOnLoad after exactly one scene switch
    /// </summary>
    /// <remarks>
    /// The parent of this object will no longer have this object as a child after a scene switch.
    /// </remarks>
    [DisallowMultipleComponent]
    public class OnSceneSwitchToggleDestroy : MonoBehaviour
    {
        [Tooltip("Number of scene switches")]
        public int switches = 1;
        /// <summary>
        /// On awake add the delegate to scene manager
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        
        private void OnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            switches--;
            if (switches >= 0) return;
            GameObject go = new GameObject("Objects that were DontDestroyOnLoad");
            // for some f---ing reason there is no method so instead you just create a new child object
            gameObject.transform.parent = go.transform;
        }
        
        /// <summary>
        /// Free resources
        /// </summary>
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
    }
}