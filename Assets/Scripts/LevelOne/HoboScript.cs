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
        public void BreakDownDoor()
        {
            Debug.Log("rip bozo");
            if (doorToBreak != null) doorToBreak.locked = false;
        }
    }
}