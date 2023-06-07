using UnityEngine;

public class CellVisualizer : MonoBehaviour {
    private MeshRenderer meshRenderer;
    
    
    public Cell gridCell;

    void Start()
    {   
        meshRenderer = GetComponent<MeshRenderer>();
        Grid.Instance.grid[gridCell.x, gridCell.y] = gridCell;
    }

    public void Enabled(bool enabled) {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = enabled;
        gridCell.isEnabled = enabled;
    }

    public void IsBlock(bool isBlock)
    {   
        meshRenderer = GetComponent<MeshRenderer>();
        gridCell.isBlock = isBlock;
        // meshRenderer.material.color = isBlock ? Color.black : Color.white;
        GameObject.Find("Grid").GetComponent<Grid>().SpawnItem(new Vector2(gridCell.x, gridCell.y), this.gameObject);
    }
    public void ActivateGravity()
    {
        this.gameObject.AddComponent<Rigidbody>();
    }
}