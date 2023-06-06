using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GameObject prefab;
    public float scale = 1f;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public int rows = 10;
    public int cols = 10;

    public GameObject[,] cells;

    public static GridVisualizer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // CreateGridVisual();
    }

    public void CreateGridVisual()
    {   
        Grid.Instance.CreateGrid();
    }

    public void SpawnCell(Cell cell)
    {
        Vector3 position = GetCellPosition(cell.x, cell.y);
        GameObject instance = Instantiate(prefab, position, Quaternion.identity, transform);
        CellVisualizer visualizer = instance.AddComponent<CellVisualizer>();
        visualizer.gridCell = cell;
        cell.visualizer = visualizer;
        instance.transform.localScale = new Vector3(scale, scale, scale);
    }

    public Vector3 GetCellPosition(int x, int y)
    {
        Vector3 position = new Vector3(x * offsetX, 0f, y * offsetY);
        return position;
    }



    // public void MovePlayer(Vector2 cell)
    // {   
    //     Vector3 position = new Vector3(cell.x * offsetX, 0f, cell.y * offsetY);
    //     PlayerMovement.Instance.MovePlayer(position);
    // }
}
