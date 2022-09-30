using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuScreen), true)]
public class MenuScreenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DrawTestButtons();
    }

    private void DrawTestButtons()
    {
        GUILayout.Space(16);

        EditorGUI.BeginDisabledGroup(Application.isPlaying == false);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Animate In", GUILayout.Height(48)))
        {
            (target as MenuScreen).AnimateIn();
        }
        if (GUILayout.Button("Animate Out", GUILayout.Height(48)))
        {
            (target as MenuScreen).AnimateOut();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
    }
}