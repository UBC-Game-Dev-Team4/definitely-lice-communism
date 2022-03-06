using System.Linq;
using DefaultNamespace;
using LevelOne;
using StageInfo;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LevelTwo
{
    /// <summary>
    /// Script to contain most information about Level 2 murder stage
    /// </summary>
    public class LevelTwoInfoStorer : NextStageInfoStorer
    {
        /// <summary>
        /// Information casted to LevelOnePreviousStageInformation
        /// </summary>
        public LevelTwoPreviousStageInformation CastedInfo
        {
            get => information as LevelTwoPreviousStageInformation;
            set => information = value;
        }

        private static LevelTwoInfoStorer _cachedSingleton;

        /// <summary>
        /// Singleton casted to LevelOneInfoStorer
        /// </summary>
        public static LevelTwoInfoStorer CastedSingleton
        {
            get
            {
                if (_cachedSingleton == null)
                {
                    _cachedSingleton = Storers.FirstOrDefault(storer => storer is LevelTwoInfoStorer) as LevelTwoInfoStorer;
                }

                return _cachedSingleton;
            }
        }

        [Tooltip("Next scene to move to")]
        public string nextScene = "Scene2Det";
        
        protected override void Awake()
        {
            base.Awake();
            if (setter != null) return;
            if (information == null) information = new LevelTwoPreviousStageInformation();
        }

        public void ApplySwitchedData(LevelOneSwitchStageInformation info)
        {
            if (info == null) return;
            if (information == null) information = new LevelTwoPreviousStageInformation();
            information.SetRespects(info.murdererRespect,info.detectiveRespect);
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