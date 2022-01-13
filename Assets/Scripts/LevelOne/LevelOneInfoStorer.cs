﻿using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LevelOne
{
    /// <summary>
    /// Script to contain most information about Level 1 murder stage
    /// </summary>
    public class LevelOneInfoStorer : NextStageInfoStorer<LevelOneInfoSetterScript>
    {
        /// <summary>
        /// Information casted to LevelOnePreviousStageInformation
        /// </summary>
        public LevelOnePreviousStageInformation CastedInfo
        {
            get => information as LevelOnePreviousStageInformation;
            set => information = value;
        }

        /// <summary>
        /// Singleton casted to LevelOneInfoStorer
        /// </summary>
        public static LevelOneInfoStorer CastedSingleton => Instance == null ? null : Instance as LevelOneInfoStorer;

        [Tooltip("Next scene to move to")]
        public string nextScene = "Scene1Det";
        [Tooltip("Event triggered on chef kill")]
        public UnityEvent eventOnKill = new UnityEvent();
        
        protected override void Start()
        {
            base.Start();
            if (setter != null) return;
            information = new LevelOnePreviousStageInformation();
        }

        /// <summary>
        /// Runs when the chef dies - sets the position and calls the eventOnKill event
        /// </summary>
        /// <param name="killPosition">Chef dead body position</param>
        public void OnKilledChef(Vector3 killPosition)
        {
            CastedInfo.deadBodyLocation = killPosition;
            eventOnKill.Invoke();
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