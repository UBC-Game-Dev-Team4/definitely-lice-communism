using UnityEngine;

namespace StageInfo
{
    /// <summary>
    /// Abstract class defining a setter that applies persistent data from the previous stage
    /// </summary>
    public abstract class PreviousStageInfoSetterScript : MonoBehaviour
    {
        /// <summary>
        /// Applies persistent information from the previous stage into this stage
        /// </summary>
        /// <param name="information">Persistent information</param>
        public abstract bool ApplyInformation(PreviousStageInformation information);
    }
}