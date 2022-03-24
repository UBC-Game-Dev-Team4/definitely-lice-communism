using System;
using DefaultNamespace;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Script attached to the hobo
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Animator))]
    public class HoboScript : MonoBehaviour
    {
        [Tooltip("Door to break down supposedly")]
        public DoorScript doorToBreak;
        private Animator _animator;
        private static readonly int WakeUpTrigger = Animator.StringToHash("WakeUpTrigger");
        private static readonly int GoToSleepTrigger = Animator.StringToHash("GoToSleepTrigger");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Break down the door
        /// </summary>
        /// <remarks>
        /// Used in an event
        /// </remarks>
        // ReSharper disable once UnusedMember.Global
        public void BreakDownDoor()
        {
            Debug.Log("rip bozo");
            if (doorToBreak != null)
            {
                doorToBreak.BreakDoor();
            }
            GoBackToSleep();
            LevelOneInfoStorer.CastedSingleton.CastedInfo.doorBroken = true;
        }

        /// <summary>
        /// Play wake up animation
        /// </summary>
        /// <remarks>Used in an event</remarks>
        // ReSharper disable once UnusedMember.Global
        public void WakeUp()
        {
            _animator.SetTrigger(WakeUpTrigger);
        }
        
        /// <summary>
        /// Go back to sleeping animation
        /// </summary>
        /// <remarks>Used in an event</remarks>
        // ReSharper disable once UnusedMember.Global
        public void GoBackToSleep()
        {
            _animator.SetTrigger(GoToSleepTrigger);
        }
    }
}