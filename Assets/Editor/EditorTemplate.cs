using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// Custom inspector for -class_name-
    /// </summary>
//[CustomEditor(typeof(-class_name-))/*, CanEditMultipleObjects*/] 
    public class EditorTemplate : UnityEditor.Editor
    {
        // ReSharper disable once UnusedMember.Local
        private static readonly string[] DontInclude = new string[] {"m_Script"};

        /// <summary>
        /// Runs upon it being shown in editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            // If not editing multiple object, one can just do script = (class_name) target;
            // DrawPropertiesExcluding(serializedObject, dontInclude);
            // class_name[] scripts = new class_name[targets.Length];
            if (GUILayout.Button("Save"))
            {
                // perform code upon button  press  
            }

            // for (int i = 0; i < targets.Length; i++) scripts[i] = (class_name)targets[i];
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
    }
}