using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;

    public VisualSettingsScriptableObject visualSettings;
    public float scale;
    public GameObject playerPrefab;
    public GameObject targetPrefab;
    public GameObject prefab;
    public float offsetX;
    public float offsetY;
    public float playerHeight;
    public float itemHeight;

    public Cell[,] grid;
    public Vector2 playerCell = new Vector2(0,0); 
    public Vector2 targetCell = new Vector2(4,4); 

    public GameObject player;
    public GameObject target;

    public static Grid Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        grid = new Cell[width, height];
        ApplyVisualSettings();
    }

    public void Spawn()
    {   
        ApplyVisualSettings();
        CreateGrid();
        player = SpawnItem(playerCell, playerPrefab);
        target = SpawnItem(targetCell, targetPrefab);
    }

    public void CreateGrid()
    {
        grid = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {   
                Cell cell =  new Cell(x,y);
                grid[x, y] = cell;
                
                SpawnCell(cell);
            }
        }
    }

    // public void CreateLevel()
    // {   
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {   
    //             if (grid[x,y].isEnabled) 
    //             {
    //                 SpawnCell(grid[x,y]);
    //             }
    //         }
    //     }
    //     player = SpawnItem(playerCell, playerPrefab);
    //     target = SpawnItem(targetCell, targetPrefab);
    // }

    public Cell GetCell(Vector2 pos)
    {
        return grid[(int)pos.x, (int)pos.y];
    }

    public void MovePlayer(Vector2 dir)
    {
        Vector2 newCell = playerCell + dir;

        if (InBounds(newCell) && GetCell(newCell).isEnabled)
        {   
            if (newCell == targetCell) 
            {
                Debug.Log("You Won");
            }

            DropCell(GetCell(playerCell));
            Assemble(new Vector3(dir.x, 0, dir.y)); 
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

    public void DropCell(Cell cell)
    {   
        cell.isEnabled = false;
        cell.visualizer.ActivateGravity();
    }

    public float _rollSpeed = 5;

    void Assemble(Vector3 dir) 
    {
        var anchor = player.transform.position + (Vector3.down + dir) * (offsetX/2) ;
        var axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));
    }
    
    private IEnumerator Roll(Vector3 anchor, Vector3 axis) {
        // _isMoving = true;
        for (var i = 0; i < 90 / _rollSpeed; i++) {
            player.transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        // _isMoving = false;
    }

    public void ApplyVisualSettings()
    {
        scale = visualSettings.scale;
        playerPrefab = visualSettings.playerPrefab;
        targetPrefab = visualSettings.targetPrefab;
        prefab = visualSettings.prefab;
        offsetX = visualSettings.offsetX;
        offsetY = visualSettings.offsetY;
        playerHeight = visualSettings.playerHeight;
        itemHeight = visualSettings.itemHeight;
    }

}
