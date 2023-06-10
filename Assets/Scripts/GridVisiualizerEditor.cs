using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid))]
public class GridVisualizerEditor : Editor {
    private Grid gridVisualizer;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        gridVisualizer = (Grid)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Grid Visual")) {
            gridVisualizer.Spawn();
        }
        if (GUILayout.Button("Save Level Grid")) {
            gridVisualizer.SaveLevelgrid();
        }
        if (GUILayout.Button("Load Level Grid")) {
            gridVisualizer.LoadLevelgrid();
        }
        // EditorGUILayout.Space();

        // EditorGUILayout.LabelField("Grid Visualization", EditorStyles.boldLabel);

        // int gridRows = gridVisualizer.rows;
        // int gridCols = gridVisualizer.cols;

        // float cellSize = 30f;
        // Rect gridRect = GUILayoutUtility.GetRect(gridCols * cellSize, gridRows * cellSize);

        // DrawGridVisual(gridRect, gridRows, gridCols, cellSize, gridVisualizer);
    }

    // private void DrawGridVisual(Rect rect, int gridRows, int gridCols, float cellSize, GridVisualizer gridVis) {
    //     for (int row = 0; row < gridRows; row++) {
    //         for (int col = 0; col < gridCols; col++) {
    //             Rect cellRect = new Rect(rect.x + col * cellSize, rect.y + row * cellSize, cellSize, cellSize);
    //             bool isEnabled = gridVis.IsCellEnabled(row, col);
    //             bool newEnabledState = EditorGUI.Toggle(cellRect, isEnabled);

    //             if (isEnabled != newEnabledState) {
    //                 gridVis.SetCellEnabled(row, col, newEnabledState);
    //             }
    //         }
    //     }
    // }
}
