using System;
using UnityEngine;

namespace DialogueStory.Actions
{
    /// <summary>
    /// Represents the ID for a dialogue action
    /// </summary>
    /// <remarks>
    /// Copied over from old code; Ask @what am i#6393
    /// </remarks>
    [Serializable]
    public struct DialogueActionId : IEquatable<DialogueActionId>
    {
        [Tooltip("ID name")]
        public string name;
        
        public bool Equals(DialogueActionId other)
        {
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            return obj is DialogueActionId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }

        public static bool operator ==(DialogueActionId left, DialogueActionId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DialogueActionId left, DialogueActionId right)
        {
            return !left.Equals(right);
        }
    }
}