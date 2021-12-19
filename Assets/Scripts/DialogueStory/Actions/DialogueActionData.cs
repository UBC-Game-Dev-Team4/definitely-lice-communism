using System;
using UnityEngine;

namespace DialogueStory.Actions
{
    /// <summary>
    /// Data to be passed to dialogue actions
    /// </summary>
    [Serializable]
    public struct DialogueActionData
    {
        [Tooltip("Data")]
        public object data;
    }
}

