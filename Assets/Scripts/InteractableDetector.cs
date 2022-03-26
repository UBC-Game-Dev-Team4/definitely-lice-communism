﻿using System.Collections.Generic;
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
        private Interactable _highlighted;
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            _list.Sort(new InteractableComparator(transform.position));
            if (_list.Count <= 0)
            {
                if (_highlighted == null) return;
                _highlighted.DeHighlight();
                _highlighted = null;
            }
            else
            {
                if (_list[0] != _highlighted)
                {
                    if (_highlighted != null)
                        _highlighted.DeHighlight();
                    _highlighted = null;
                }

                _list[0].Highlight();
                _highlighted = _list[0];
            }
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
                if (priorityDiff != 0) return -priorityDiff; // priority must be flipped????????
                Vector3 xdiff = x.transform.position - _position;
                Vector3 ydiff = y.transform.position - _position;
                // sqrMagnitude is faster than magnitude due to the lack of sqrt call
                return xdiff.sqrMagnitude.CompareTo(ydiff.sqrMagnitude);
            }
        }
    }
}