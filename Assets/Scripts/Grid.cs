using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;

    public VisualSettingsScriptableObject visualSettings;
    public GridSettingsScriptableObject gridSettings;
    public float scale;
    public Dictionary<string, GameObject> pool;
    public GameObject playerPrefab;
    public GameObject targetPrefab;
    public GameObject prefab;
    public float offsetX;
    public float offsetY;
    public float playerHeight;
    public float itemHeight;
    public Material glassMaterial;
    public List<Material> materials;

    public Dictionary <int, List<CellVisualizer>> morphCellDict;

    public CellArray2D grid;
    public Vector2 playerCell; 
    public Vector2 targetCell; 

    private GameObject player;
    private GameObject target;

    [Serializable]
    public class Prefab
    {
        public GameObject prefabObject;
        public float height;
        public string tag;
    }

    public List<Prefab> prefabList = new List<Prefab>();
    public Dictionary<string, Prefab> prefabDictionary = new Dictionary<string, Prefab>();

    public static Grid Instance { get; private set; }
    
    private void Awake()
    {   
        InitializeSingleton();
        grid = new CellArray2D(width, height);
        morphCellDict = new Dictionary <int, List<CellVisualizer>>();
        PopulateDictionary();
        ApplyVisualSettings();
        LoadLevelgrid();
        player = SpawnItem("player", playerCell);
        target = SpawnItem("target", targetCell);
        
    }

    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SaveLevelgrid()
    {   
        InitializeSingleton();

        foreach (Cell cell in grid.cells)
        {   
            cell.visualizer.SaveCell();
        }

        gridSettings.SaveCurrentSettings(this);
        visualSettings.grid = grid;
        visualSettings.morphCellDict = morphCellDict;
        visualSettings.playerCell = playerCell;
        visualSettings.targetCell = targetCell;
    }

    
    public void LoadLevelgrid()
    {   
        grid = visualSettings.grid;
        foreach (Cell cell in grid.cells)
        {   
            SpawnLevelCell(cell);
        }
        // morphCellDict = visualSettings.morphCellDict;
        playerCell = visualSettings.playerCell;
        targetCell = visualSettings.targetCell;
    }

    private void PopulateDictionary()
    {
        prefabDictionary.Clear();

        foreach (Prefab prefab in prefabList)
        {
            if (!prefabDictionary.ContainsKey(prefab.tag))
            {
                prefabDictionary.Add(prefab.tag, prefab);
            }
            else
            {
                Debug.LogWarning("Prefab tag " + prefab.tag + " already exists in the dictionary.");
            }
        }
    }

    public void Spawn()
    {   
        InitializeSingleton();
        PopulateDictionary();
        ApplyVisualSettings();
        CreateGrid();
        player = SpawnItem("player", playerCell);
        target = SpawnItem("target", targetCell);
    }

    public void CreateGrid()
    {
        grid = new CellArray2D(width, height);
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

            if (GetCell(playerCell).isGlass)
            {
                GetCell(playerCell).DropCell();
            }

            Assemble(new Vector3(dir.x, 0, dir.y)); 
            playerCell = newCell;

            if (GetCell(playerCell).isButton)
            {   
                
                ToggleCells(GetCell(playerCell).morphIndex);
            }
        }
        return;
    }

    public void ToggleCells(int tag)
    {
        if (morphCellDict != null && morphCellDict.ContainsKey(tag))
        {
            List<CellVisualizer> cells = morphCellDict[tag];

            foreach (CellVisualizer cell in cells)
            {   
                cell.ToggleSize();
                Debug.Log("toggle cell");
            }
        }
        else
        {
            Debug.LogWarning("No cells found for tag " + tag);
        }
    }


    public bool InBounds(Vector2 position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    public void SpawnLevelCell(Cell cell)
    { 
        Vector3 position = new Vector3(cell.x * offsetX, 0, cell.y * offsetY);
        GameObject instance = Instantiate(prefab, position, Quaternion.identity, transform);
        CellVisualizer visualizer = instance.AddComponent<CellVisualizer>();
        visualizer.gridCell = cell;
        cell.visualizer = visualizer;
        visualizer.TransformCell();
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

    public GameObject SpawnItem(string tag, Vector2 pos)
    {
        if (prefabDictionary.ContainsKey(tag))
        {
            Prefab prefab = prefabDictionary[tag];
            Vector3 position = new Vector3(pos.x * offsetX, prefab.height, pos.y * offsetY);
            GameObject instance = Instantiate(prefab.prefabObject, position, Quaternion.identity, transform);
            return instance;
        }
        else
        {
            Debug.LogWarning("Prefab with tag " + tag + " does not exist in the dictionary.");
            return null;
        }
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

    public void AddToMorphableList(int key, CellVisualizer cellVisualizer)
    {
        if (morphCellDict.ContainsKey(key))
        {
            morphCellDict[key].Add(cellVisualizer);
        }
        else
        {
            morphCellDict[key] = new List<CellVisualizer> { cellVisualizer };
        }
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
        materials = visualSettings.materials;
    }
}
