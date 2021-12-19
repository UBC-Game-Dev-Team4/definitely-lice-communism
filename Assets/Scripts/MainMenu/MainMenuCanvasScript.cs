using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    /// <summary>
    ///     Script attached to the title canvas - used to load a scene
    /// </summary>
    [DisallowMultipleComponent]
    public class MainMenuCanvasScript : MonoBehaviour
    {
        /// <summary>
        ///     Loads a scene by given name, if it exists
        /// </summary>
        /// <param name="scene">Name of scene</param>
        /// <remarks>
        ///     If the scene doesn't load, try adding it to the list of scenes to build in build settings.
        /// </remarks>
        public void LoadScene(string scene)
        {
            if (Application.CanStreamedLevelBeLoaded(scene))
                SceneManager.LoadScene(scene);
            else
                Debug.LogError("Could not load scene: " + scene);
        }
    }
}