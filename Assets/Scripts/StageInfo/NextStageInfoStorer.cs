using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StageInfo
{
    /// <summary>
    /// Stores persistent information of the current stage for the next stage
    /// </summary>
    public class NextStageInfoStorer : MonoBehaviour
    {
        public static readonly List<NextStageInfoStorer> Storers = new List<NextStageInfoStorer>();
        /// <summary>
        /// Information being stored
        /// </summary>
        public PreviousStageInformation information;
        protected PreviousStageInfoSetterScript setter;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            Storers.Add(this);
            OnSceneChange();
            if (setter == null) SceneManager.activeSceneChanged += OnSceneChange;
        }

        protected virtual void OnSceneChange()
        {
            PreviousStageInfoSetterScript[] setters = FindObjectsOfType<PreviousStageInfoSetterScript>();
            if (setters == null)
            {
                Debug.Log("Cannot find setter of type" + typeof(PreviousStageInfoSetterScript));
                return;
            }
            Debug.Log(setters.Length + " setters found.");
            foreach (PreviousStageInfoSetterScript possibleSetter in setters)
            {
                if (!possibleSetter.ApplyInformation(information)) continue;
                Storers.Remove(this);
                Destroy(this.gameObject);
                Debug.Log("Setter found.");
                setter = possibleSetter;
            }
        }
        
        protected virtual void OnSceneChange(Scene current, Scene next)
        {
            OnSceneChange();
        }
    }
}