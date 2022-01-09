using System;
using DefaultNamespace;
using UnityEngine;

namespace LevelOne
{
    public class LevelOneInfoSetterScript : PreviousStageInfoSetterScript
    {
        public GameObject deadChefGameObject;
        private LevelOnePreviousStageInformation _info;
        public override void ApplyInformation(PreviousStageInformation information)
        {
            if (!(information is LevelOnePreviousStageInformation levelOneInfo))
                throw new ArgumentException("LevelOneInfoSetterScript needs LevelOnePreviousStageInformation object");

            _info = levelOneInfo;
            if (deadChefGameObject != null) deadChefGameObject.transform.position = levelOneInfo.deadBodyLocation;
        }
    }
}