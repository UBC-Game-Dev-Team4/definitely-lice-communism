using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    /// <summary>
    /// Stores persistent information of the current stage for the next stage
    /// </summary>
    /// <typeparam name="T">Current type</typeparam>
    public class NextStageInfoStorer : Singleton<NextStageInfoStorer>
    {
        /// <summary>
        /// Information being stored
        /// </summary>
        public PreviousStageInformation information;
        protected PreviousStageInfoSetterScript setter;
        protected virtual void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            OnSceneChange();
            if (setter == null) SceneManager.activeSceneChanged += OnSceneChange;
        }

        protected virtual void OnSceneChange()
        {
            setter = FindObjectOfType<PreviousStageInfoSetterScript>();
            if (setter == null)
            {
                Debug.Log("Cannot find setter of type" + typeof(PreviousStageInfoSetterScript));
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