using LevelOne;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ChefScript))]
    public class ChefScriptEditor : AIScriptEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Test Move From Kitchen")) ((ChefScript) target).MoveFromKitchen();
        }
    }
}