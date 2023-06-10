using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid))]
public class GridVisualizerEditor : Editor {
    private Grid gridVisualizer;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        gridVisualizer = (Grid)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Create a new Grid")) {
            gridVisualizer.Spawn();
        }
        if (GUILayout.Button("Save current Grid")) {
            gridVisualizer.SaveLevelgrid();
        }
        if (GUILayout.Button("Load saved Grid")) {
            gridVisualizer.LoadLevelgrid();
        }
    }
}
