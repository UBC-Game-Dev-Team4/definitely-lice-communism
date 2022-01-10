using DefaultNamespace;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Container for information about Level 1 murder stage
    /// </summary>
    [System.Serializable]
    public class LevelOnePreviousStageInformation : PreviousStageInformation
    {
        /// <summary>
        /// Whether the chef was killed via oil
        /// </summary>
        [SerializeField]
        public bool wasKilledViaHotOil = false;
        /// <summary>
        /// Dead body location
        /// </summary>
        [SerializeField]
        public Vector3 deadBodyLocation;
    }
}