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

        if (GUILayout.Button("Is Glass")) {
            cellVisualizer.IsGlass(true);
        }
        if (GUILayout.Button("Is not Glass")) {
            cellVisualizer.IsGlass(false);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Is Morphing Button")) {
            cellVisualizer.IsButton(true);
        }
        if (GUILayout.Button("Is not Morphing Button")) {
            cellVisualizer.IsButton(false);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Is Spike")) {
            cellVisualizer.IsSpike(true);
        }
        if (GUILayout.Button("Is not Spike")) {
            cellVisualizer.IsSpike(false);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Is Morphable")) {
            cellVisualizer.IsMorphable(true);
        }
        if (GUILayout.Button("Is not Morphable")) {
            cellVisualizer.IsMorphable(false);
        }

        if (GUILayout.Button("Toggle Morph size")) {
            cellVisualizer.ToggleSize();;
        }
    }
}
