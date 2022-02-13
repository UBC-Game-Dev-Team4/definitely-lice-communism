using ItemInventory;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// Custom inspector for <see cref="ReputationMeter"/>
    /// </summary>
    [CustomEditor(typeof(ReputationMeter))]
    public class ReputationMeterEditor : UnityEditor.Editor
    {
        private static readonly string[] DontInclude = new string[] {"m_Script"};

        /// <summary>
        /// Runs upon it being shown in editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawPropertiesExcluding(serializedObject, DontInclude);
            ReputationMeter script = (ReputationMeter) target;
            if (GUILayout.Button("Recreate bar"))
            {
                script.RecreateBar();
            }

            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
    }
}