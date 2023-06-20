using UnityEngine;
[System.Serializable]
public class Cell 
{
    public bool isEnabled;
    public bool isBlock;
    public bool isGlass;
    public bool isSpike;
    public bool isMorphable;
    public bool isButton;
    public int morphIndex;
    public CellVisualizer visualizer;
    public GameObject cellGameObject;
    public Vector3 position;
    public Vector3 scale;
    public Vector2 vector;
    public MeshRenderer renderer;
    public int x;
    public int y;

    public Cell(int _x, int _y) {
        x = _x;
        y = _y;
        isEnabled = true;
        isBlock = false;
        vector = new Vector2(x, y);
    }

    public void DropCell()
    {   
        isEnabled = false;
        visualizer.ActivateGravity();
    }
}