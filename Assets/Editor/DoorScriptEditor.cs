using DefaultNamespace;
using UnityEditor;

namespace Editor
{
    /// <summary>
    /// Custom inspector for <see cref="DoorScript"/>
    /// </summary>
    [CustomEditor(typeof(DoorScript)), CanEditMultipleObjects]
    public class DoorScriptEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Runs upon it being shown in editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            if (targets.Length == 1)
            {
                DoorScript doorScript = (DoorScript) targets[0];
                DrawPropertiesExcluding(serializedObject,
                    doorScript.shouldPopCameraStateOnInteract
                        ? new[] {"m_Script", nameof(DoorScript.cameraStateOnInteract)}
                        : new[] {"m_Script"});
            }
            else
            {
                DrawPropertiesExcluding(serializedObject,new[]{"m_Script"});
            }
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
    }
}