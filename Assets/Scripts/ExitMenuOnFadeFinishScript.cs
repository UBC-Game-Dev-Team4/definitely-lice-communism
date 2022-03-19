using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    /// <summary>
    /// Script to listen for the OnFadeFinish event called by an animation
    /// </summary>
    /// <remarks>
    /// Sets all game objects in an array active upon fade completion
    /// </remarks>
    public class ExitMenuOnFadeFinishScript : MonoBehaviour
    {
        [Tooltip("Enable these objects upon fade completion")]
        public GameObject[] objectsToEnable;

        public UnityEvent eventOnFinish;

        // ReSharper disable once UnusedMember.Global
        public void OnFadeFinish()
        {
            foreach (GameObject go in objectsToEnable)
            {
                if (go != null) go.SetActive(true);
            }

            eventOnFinish?.Invoke();
        }
    }
}