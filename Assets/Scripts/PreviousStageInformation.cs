using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Abstract class containing some shared stage information
    /// </summary>
    [System.Serializable]
    public abstract class PreviousStageInformation
    {
        [SerializeField]
        public int detectiveRespect = 50;
        [SerializeField]
        public int murdererRespect = 50;
    }
}