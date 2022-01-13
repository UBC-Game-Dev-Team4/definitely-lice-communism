using UnityEngine;

namespace DefaultNamespace
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
        public abstract void ApplyInformation(PreviousStageInformation information);
    }
}