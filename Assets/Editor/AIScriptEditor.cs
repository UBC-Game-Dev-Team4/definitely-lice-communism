using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// Custom inspector for <see cref="AIScript"/>
    /// </summary> 
    [CustomEditor(typeof(AIScript),true), CanEditMultipleObjects]
    public class AIScriptEditor : UnityEditor.Editor
    {
        private static readonly string[] dontInclude = new string[] {"m_Script"};

        /// <summary>
        /// Runs upon it being shown in editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            // If not editing multiple object, one can just do script = (class_name) target;
            if (targets.Length > 1) DrawPropertiesExcluding(serializedObject, dontInclude);
            else
            {
                AIScript script = (AIScript) target;
                switch (script.mode)
                {
                    case AIMode.Stationary:
                        DrawPropertiesExcluding(serializedObject, "m_Script", nameof(script.acceleration),
                            nameof(script.deceleration), nameof(script.maxSpeed), nameof(script.directionIsLeft),
                            nameof(script.maxWanderDuration), nameof(script.minWanderDuration),
                            nameof(script.maxDelayBeforeWander), nameof(script.minDelayBeforeWander),
                            nameof(script.wanderLockXLeft), nameof(script.wanderLockXRight));
                        break;
                    case AIMode.Wander:
                        DrawPropertiesExcluding(serializedObject, "m_Script",
                            nameof(script.wanderLockXLeft), nameof(script.wanderLockXRight), nameof(script.directionIsLeft));
                        break;
                    case AIMode.WanderLocked:
                        DrawPropertiesExcluding(serializedObject, "m_Script", nameof(script.directionIsLeft));
                        break;
                    case AIMode.SpecificDirection:
                        DrawPropertiesExcluding(serializedObject, "m_Script",
                            nameof(script.maxWanderDuration), nameof(script.minWanderDuration),
                            nameof(script.maxDelayBeforeWander), nameof(script.minDelayBeforeWander),
                            nameof(script.wanderLockXLeft), nameof(script.wanderLockXRight));
                        break;
                }
                if (GUILayout.Button("Properly Set Listed AImode"))
                {
                    script.SetMode(script.mode);
                }
            }
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
    }
}