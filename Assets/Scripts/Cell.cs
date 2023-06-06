using UnityEngine;
[System.Serializable]
public class Cell {
    
    public bool isEnabled;
    public bool isBlock;
    public CellVisualizer visualizer;
    public GameObject cellGameObject;
    public int x;
    public int y;

    public Cell(int _x, int _y) {
        x = _x;
        y = _y;
        isEnabled = true;
        isBlock = false;
    }
}