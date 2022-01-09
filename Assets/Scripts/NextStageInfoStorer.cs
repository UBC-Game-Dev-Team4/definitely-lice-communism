using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class NextStageInfoStorer<T> : Singleton<NextStageInfoStorer<T>> where T: PreviousStageInfoSetterScript
    {
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