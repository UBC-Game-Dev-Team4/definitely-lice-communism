using System;
using DefaultNamespace;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Script that sets various information from Level 1 murder stage in level 1 detective stage
    /// </summary>
    public class LevelOneInfoSetterScript : PreviousStageInfoSetterScript
    {
        [Tooltip("Dead chef game object to move")]
        public GameObject deadChefGameObject;
        private LevelOnePreviousStageInformation _info;
        /// <inheritdoc/>
        public override void ApplyInformation(PreviousStageInformation information)
        {
            if (!(information is LevelOnePreviousStageInformation levelOneInfo))
                throw new ArgumentException("LevelOneInfoSetterScript needs LevelOnePreviousStageInformation object");

            _info = levelOneInfo;
            if (deadChefGameObject != null) deadChefGameObject.transform.position = _info.deadBodyLocation;
        }
    }
}