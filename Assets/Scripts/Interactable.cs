using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    /// <summary>
    ///     Script attached to an object that can be interacted with
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        [Tooltip("Event(s) that run upon interaction")]
        public UnityEvent eventOnInteract = new UnityEvent();

        [Tooltip("Priority of interaction (used in sorting order for player interaction)")]
        public int priority;

        /// <summary>
        /// Whether the current object is highlighted
        /// </summary>
        public bool Highlighted { get; protected set; }

        /// <summary>
        /// Indicate the object is being highlighted/active.
        /// </summary>
        public virtual void Highlight()
        {
            Highlighted = true;
        }

        /// <summary>
        /// Indicate the object is no longer being highlighted.
        /// </summary>
        public virtual void DeHighlight()
        {
            Highlighted = false;
        }
        
        /// <summary>
        ///     Interact with this object by triggering the events on interact
        /// </summary>
        /// <param name="src">Object that interacted with this object</param>
        /// <param name="args">Optional arguments</param>
        public virtual void Interact(object src, params object[] args)
        {
            eventOnInteract.Invoke();
        }
    }
}