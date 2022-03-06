using UnityEngine;

namespace StageInfo
{
    /// <summary>
    /// Abstract class containing some shared stage information
    /// </summary>
    [System.Serializable]
    public abstract class PreviousStageInformation
    {
        /// <summary>
        /// On destructor called, clear event
        /// </summary>
        ~PreviousStageInformation()
        {
            Debug.Log("Cleaning up PreviousStageInformation...");
            RespectChange = null;
        }
        
        /// <summary>
        /// Delegate used for event
        /// </summary>
        public delegate void RespectChangeHandler(PreviousStageInformation source);

        /// <summary>
        /// Event called on non-zero respect change
        /// </summary>
        public event RespectChangeHandler RespectChange;
        /// <summary>
        /// Total amount of respect
        /// </summary>
        public static readonly int TotalRespect = 100;
        [SerializeField]
        public int detectiveRespect = TotalRespect/2;
        [SerializeField]
        public int murdererRespect = TotalRespect/2;

        /// <summary>
        /// Manually trigger refresh
        /// </summary>
        public void TriggerRefreshManual()
        {
            RespectChange?.Invoke(this);
        }

        /// <summary>
        /// Sets the murder and detective respect
        /// </summary>
        /// <param name="mur">New murder respect</param>
        /// <param name="det">New detective respect</param>
        public virtual void SetRespects(int mur, int det)
        {
            bool shouldTrigger = murdererRespect != mur || detectiveRespect != det;
            murdererRespect = mur;
            detectiveRespect = det;
            
            if (detectiveRespect <= 0 || murdererRespect <= 0)
            {
                OnZeroOrNegativeRespect();
            }
            if (shouldTrigger) RespectChange?.Invoke(this);
        }

        /// <summary>
        /// Adds murder respect and removes detective respect
        /// </summary>
        /// <param name="amount">Respect to add</param>
        public virtual void AddMurderRespect(int amount)
        {
            murdererRespect += amount;
            detectiveRespect -= amount;
            if (detectiveRespect <= 0 || murdererRespect <= 0)
            {
                OnZeroOrNegativeRespect();
            }
            if (amount != 0) RespectChange?.Invoke(this);
            Debug.Assert(murdererRespect + detectiveRespect == TotalRespect);
        }
        
        /// <summary>
        /// Adds detective respect (and removes murder respect)
        /// </summary>
        /// <param name="amount">Respect to add</param>
        public virtual void AddDetectiveRespect(int amount)
        {
            detectiveRespect += amount;
            murdererRespect -= amount;
            if (detectiveRespect <= 0 || murdererRespect <= 0)
            {
                OnZeroOrNegativeRespect();
            }
            if (amount != 0) RespectChange?.Invoke(this);
            Debug.Assert(murdererRespect + detectiveRespect == TotalRespect);
        }

        /// <summary>
        /// Called on zero/negative respect.
        ///
        /// TODO IMPLEMENTATION
        /// </summary>
        public virtual void OnZeroOrNegativeRespect()
        {
            Debug.Log("Hey! detectiveRespect/murdererRespect < 0!!!!");
            // TODO LOSS
        }
    }
}