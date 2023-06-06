using UnityEditor;
using UnityEngine;

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

        if (GUILayout.Button("It is not block")) {
            cellVisualizer.IsBlock(false);
        }

        if (GUILayout.Button("Is Block")) {
            cellVisualizer.IsBlock(true);
        }
    }
}
