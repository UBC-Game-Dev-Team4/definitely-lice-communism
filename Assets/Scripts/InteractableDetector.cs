using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Script to detect the presence of interactable objects
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class InteractableDetector : Interactable
    {
        private Collider2D _collider;
        /// <summary>
        /// List of interactable items within range
        /// </summary>
        private readonly List<Interactable> _list = new List<Interactable>();

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            Interactable prevInteractable = null;
            if (_list.Count > 0) prevInteractable = _list[0];
            _list.Sort(new InteractableComparator(transform.position));
            if (_list.Count <= 0) return;
            if (_list[0] == prevInteractable)
            {
#if UNITY_EDITOR
                Debug.Assert(prevInteractable != null);
#endif
                if (!prevInteractable.Highlighted)
                {
                    prevInteractable.Highlight();
                }

                return;
            }
#if UNITY_EDITOR
            Debug.Assert(prevInteractable != null);
#endif
            prevInteractable.DeHighlight();
            _list[0].Highlight();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null) _list.Add(interactable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null) _list.Remove(interactable);
        }
        
        /// <inheritdoc cref="Interactable.Interact"/>
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            if (_list.Count > 0) _list[0].Interact(src, args);
        }

        /// <summary>
        /// Custom comparator to compare interactable objects by distance from a point
        /// </summary>
        private class InteractableComparator : IComparer<Interactable>
        {
            /// <summary>
            /// Distance to compare to
            /// </summary>
            private readonly Vector3 _position;

            /// <summary>
            /// Constructor with given point to find distance to
            /// </summary>
            /// <param name="referencePos">Point to find distance to</param>
            public InteractableComparator(Vector3 referencePos)
            {
                _position = referencePos;
            }

            /// <inheritdoc cref="IComparer{T}.Compare"/>
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