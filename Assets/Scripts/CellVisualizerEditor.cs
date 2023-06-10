using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CellVisualizer))]
public class CellVisualizerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        CellVisualizer cellVisualizer = (CellVisualizer)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Disable")) {
            cellVisualizer.Enabled(false);
        }

        if (GUILayout.Button("Enable")) {
            cellVisualizer.Enabled(true);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Button")) {
            cellVisualizer.IsButton();
        }

        if (GUILayout.Button("Is Glass")) {
            cellVisualizer.IsGlass(true);
        }

        if (GUILayout.Button("Is Morphable")) {
            cellVisualizer.IsMorphable();
        }
        if (GUILayout.Button("toggle Morph size")) {
            cellVisualizer.ToggleSize();;
        }
    }
}
