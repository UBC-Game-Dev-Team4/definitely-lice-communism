using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    /// <summary>
    /// Stores persistent information of the current stage for the next stage
    /// </summary>
    /// <typeparam name="T">Current type</typeparam>
    public class NextStageInfoStorer<T> : Singleton<NextStageInfoStorer<T>> where T: PreviousStageInfoSetterScript
    {
        /// <summary>
        /// Information being stored
        /// </summary>
        public PreviousStageInformation information;
        protected T setter;
        protected virtual void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            OnSceneChange();
            if (setter == null) SceneManager.activeSceneChanged += OnSceneChange;
        }

        protected virtual void OnSceneChange()
        {
            setter = FindObjectOfType<T>();
            if (setter == null)
            {
                Debug.Log("Cannot find setter of type" + typeof(T));
                return;
            }
            Debug.Log("Setter found.");
            setter.ApplyInformation(information);
            Destroy(this.gameObject);
        }
        
        protected virtual void OnSceneChange(Scene current, Scene next)
        {
            OnSceneChange();
        }
    }
}