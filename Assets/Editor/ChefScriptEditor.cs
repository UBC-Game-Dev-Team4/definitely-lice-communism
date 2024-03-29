﻿using LevelOne;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ChefScript),true)]
    public class ChefScriptEditor : AIScriptEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Test Move From Kitchen")) ((ChefScript) target).MoveFromKitchen();
            if (GUILayout.Button("Kill (mallet)")) ((ChefScript) target).Kill(false);
            if (GUILayout.Button("Kill (oil)")) ((ChefScript) target).Kill(true);
        }
    }
}