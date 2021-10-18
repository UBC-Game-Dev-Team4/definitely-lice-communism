using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Interactable : MonoBehaviour
    {
        public UnityEvent eventOnInteract = new UnityEvent();
        public int priority = 0;
        public virtual void Interact(object src, params object[] args)
        {
            eventOnInteract.Invoke();
        }
    }
}