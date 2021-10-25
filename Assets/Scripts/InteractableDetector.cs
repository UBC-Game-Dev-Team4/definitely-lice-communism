using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableDetector : Interactable
    {
        private List<Interactable> list = new List<Interactable>();
        private Collider2D _collider;
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            list.Sort(new InteractableComparator(transform.position));
            if (list.Count > 0) list[0].Interact(src, args);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null) list.Add(interactable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null) list.Remove(interactable);
        }

        private class InteractableComparator : IComparer<Interactable>
        {
            private readonly Vector3 _position;

            public InteractableComparator(Vector3 referencePos)
            {
                _position = referencePos;
            }

            public int Compare(Interactable x, Interactable y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                int priorityDiff = x.priority.CompareTo(y.priority);
                if (priorityDiff != 0) return priorityDiff;
                Vector3 xdiff = x.transform.position - _position;
                Vector3 ydiff = y.transform.position - _position;
                // sqrMagnitude is faster than magnitude due to the lack of sqrt call
                return xdiff.sqrMagnitude.CompareTo(ydiff.sqrMagnitude);
            }
        }
    }
}