using DefaultNamespace;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Script attached to the hobo
    /// </summary>
    [DisallowMultipleComponent]
    public class HoboScript : MonoBehaviour
    {
        [Tooltip("Door to break down supposedly")]
        public DoorScript doorToBreak;

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
            LevelOneInfoStorer.CastedSingleton.CastedInfo.doorBroken = true;
        }
    }
}