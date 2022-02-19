using System.Linq;
using StageInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelOne
{
    /// <summary>
    /// Script to contain most information about Level 1 detective stage
    /// </summary>
    public class LevelOneSwitchInfoStorer : NextStageInfoStorer
    {
        /// <summary>
        /// Information casted to LevelOnePreviousStageInformation
        /// </summary>
        public LevelOneSwitchStageInformation CastedInfo
        {
            get => information as LevelOneSwitchStageInformation;
            set => information = value;
        }

        private static LevelOneSwitchInfoStorer _cachedSingleton;

        /// <summary>
        /// Singleton casted to LevelOneInfoStorer
        /// </summary>
        public static LevelOneSwitchInfoStorer CastedSingleton
        {
            get
            {
                if (_cachedSingleton == null)
                    _cachedSingleton = Storers.FirstOrDefault(storer => storer is LevelOneSwitchInfoStorer) as LevelOneSwitchInfoStorer;
                return _cachedSingleton;
            }
        }

        [Tooltip("Next scene to move to")]
        public string nextScene = "Scene2";
        
        protected override void Awake()
        {
            base.Awake();
            if (setter != null) return;
            information = new LevelOneSwitchStageInformation();
        }
        
        /// <summary>
        /// Loads the next scene
        /// </summary>
        public void LoadNextScene()
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}