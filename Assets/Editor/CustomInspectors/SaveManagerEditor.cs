using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DrawTestButtons();
    }

    private void DrawTestButtons()
    {
        GUILayout.Space(16);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Reset SaveData", GUILayout.Height(32)))
        {
            #if UNITY_EDITOR
            (target as SaveManager).ResetSaveEditor();
            #endif
        }

        EditorGUILayout.EndHorizontal();
    }
}