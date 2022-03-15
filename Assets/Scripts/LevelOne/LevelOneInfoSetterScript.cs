using System;
using DefaultNamespace;
using StageInfo;
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

        [Tooltip("Cats in Kitchen")]
        public GameObject kitchenCattos;
        [Tooltip("Cats in Storage Room")]
        public GameObject backroomCattos;
        private LevelOnePreviousStageInformation _info;
        /// <inheritdoc/>
        public override bool ApplyInformation(PreviousStageInformation information)
        {
            if (!(information is LevelOnePreviousStageInformation levelOneInfo))
                return false;

            _info = levelOneInfo;
            LevelOneSwitchInfoStorer.CastedSingleton?.SetData(_info);
            if (deadChefGameObject != null) deadChefGameObject.transform.position = _info.deadBodyLocation;
            if (_info.deadBodyLocation.y > 5) kitchenCattos.SetActive(true);
            else backroomCattos.SetActive(true);
            return true;
        }
    }
}