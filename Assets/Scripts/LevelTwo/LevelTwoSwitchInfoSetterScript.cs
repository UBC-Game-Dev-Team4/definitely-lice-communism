using System;
using LevelOne;
using StageInfo;

namespace LevelTwo
{
    /// <summary>
    /// Script that sets various information from Level 1 detective stage in level 2 murder stage
    /// </summary>
    public class LevelTwoSwitchInfoSetterScript : PreviousStageInfoSetterScript
    {
        private LevelOneSwitchStageInformation _info;
        /// <inheritdoc/>
        public override bool ApplyInformation(PreviousStageInformation information)
        {
            if (!(information is LevelOneSwitchStageInformation levelOneInfo))
                return false;
            _info = levelOneInfo;
            LevelTwoInfoStorer storer = LevelTwoInfoStorer.CastedSingleton;
            if (storer != null)
            {
                storer.ApplySwitchedData(_info);
            }
            return true;
        }
    }
}