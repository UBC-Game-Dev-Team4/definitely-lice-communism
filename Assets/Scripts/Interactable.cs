using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    /// <summary>
    /// Script attached to an object that can be interacted with
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        [Tooltip("Event(s) that run upon interaction")]
        public UnityEvent eventOnInteract = new UnityEvent();
        [Tooltip("Priority of interaction (used in sorting order for player interaction)")]
        public int priority = 0;
        /// <summary>
        /// Interact with this object by triggering the events on interact
        /// </summary>
        /// <param name="src">Object that interacted with this object</param>
        /// <param name="args">Optional arguments</param>
        public virtual void Interact(object src, params object[] args)
        {
            eventOnInteract.Invoke();
        }
    }
}