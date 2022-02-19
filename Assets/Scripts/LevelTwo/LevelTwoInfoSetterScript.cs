using System;
using DefaultNamespace;
using StageInfo;

namespace LevelTwo
{
    /// <summary>
    /// Script that sets various information from Level 2 murder stage in level 2 detective stage
    /// </summary>
    public class LevelTwoInfoSetterScript : PreviousStageInfoSetterScript
    {
        private LevelTwoPreviousStageInformation _info;
        /// <inheritdoc/>
        public override bool ApplyInformation(PreviousStageInformation information)
        {
            if (!(information is LevelTwoPreviousStageInformation levelOneInfo))
                return false;
            _info = levelOneInfo;
            return true;
        }
    }
}