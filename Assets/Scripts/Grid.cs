using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;
    public float scale = 1f;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public Cell[,] cells;
    public Vector2 playerCell = new Vector2(0,0); 
    public Vector2 targetPos = new Vector2(4,4); 
    public GameObject playerPrefab;
    public GameObject targetPrefab;
    public GameObject prefab;
    public GameObject player;
    public GameObject target;
    public float playerHeight;
    public float itemHeight;
    public float speed;

    public static Grid Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void Start()
    {
        CreateGrid();
        player = SpawnItem(playerCell, playerPrefab);
        target = SpawnItem(targetPos, targetPrefab);
    }  

    public void Spawn()
    {
        CreateGrid();
        player = SpawnItem(playerCell, playerPrefab);
        target = SpawnItem(targetPos, targetPrefab);
    }
    public void CreateGrid()
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {   
                Cell cell =  new Cell(x,y);
                cells[x, y] = cell;

                SpawnCell(cell);
            }
        }
    }

    public Cell GetCell(Vector2 pos)
    {
        return cells[(int)pos.x, (int)pos.y];
    }

    public void MovePlayer(Vector2 dir)
    {
        Vector2 newCell = playerCell + dir;

        if (InBounds(newCell) && GetCell(newCell).isEnabled)
        {
            DropCell(GetCell(playerCell));
            MovePlayerGameObject(newCell);
            playerCell = newCell;
        }
        Debug.Log("Out of bounds");
        return;
    }

    public bool InBounds(Vector2 position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    public void SpawnCell(Cell cell)
    {
        Vector3 position = new Vector3(cell.x * offsetX, 0f, cell.y * offsetY);
        GameObject instance = Instantiate(prefab, position, Quaternion.identity, transform);
        instance.transform.localScale = new Vector3(scale, scale, scale);

        CellVisualizer visualizer = instance.AddComponent<CellVisualizer>();
        visualizer.gridCell = cell;
        cell.visualizer = visualizer;
        cell.cellGameObject = instance;
    }

    public GameObject SpawnItem(Vector2 pos, GameObject prefab)
    {   
        Vector3 position = new Vector3(pos.x * offsetX, itemHeight, pos.y * offsetY);
        GameObject instance = Instantiate(prefab, position, Quaternion.identity, transform);
        return instance;
    }

    public void MovePlayerGameObject(Vector2 newCell)
    {   
        Vector3 newPosition = new Vector3(newCell.x * offsetX, playerHeight, newCell.y * offsetY);
        float time = 0;

        while (time < 1)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, newPosition, time);
            time += Time.deltaTime * speed;
        }  
    }

    public void DropCell(Cell cell)
    {   
        cell.isEnabled = false;
        cell.visualizer.ActivateGravity();

        // Transform cellTransform = cell.cellGameObject.transform;

        // Vector3 newPosition = new Vector3(cellTransform.position.x, -20, cellTransform.position.z);
        // float time = 0;

        // while (time < 1)
        // {
        //     cellTransform.position = Vector3.Lerp(cellTransform.position, newPosition, time);
        //     time += Time.deltaTime * speed;
        // }  
    }
}
