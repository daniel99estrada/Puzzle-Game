using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid : MonoBehaviour
{
    [Header("Settings")]
    public VisualSettingsScriptableObject visualSettings;
    public GridSettingsScriptableObject gridSettings;

    [Header("Grid Dimensions")]
    public int width;
    public int height;

    [Header("Offset")]
    public float offsetX;
    public float offsetY;

    [Header("Player and Target Cells")]
    public Vector2 playerCell; 
    public Vector2 targetCell; 

    [Header("Materials")]
    private List<Material> materials;

    [Header("Cell Dictionary")]
    public Dictionary <int, List<CellVisualizer>> morphCellDict;

    [Header("Grid Container")]
    private GameObject GridContainer;

    [Header("Grid")]
    public CellArray2D grid;

    [Header("Player and Target GameObjects")]
    private GameObject player;
    private GameObject target;

    [Header("Prefab Dictionary")]
    public Dictionary<string, PrefabObject> prefabDictionary = new Dictionary<string, PrefabObject>();

    public static Grid Instance { get; private set; }
    
    public event Action OnReachedTarget;

    private void Awake()
    {   
        LoadLevelgrid();
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

    public void DestroyCells()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void SaveLevelgrid()
    {   
        InitializeSingleton();

        foreach (Cell cell in grid.cells)
        {   
            cell.visualizer.SaveCell();
        }

        gridSettings.SaveCurrentSettings(this);
    }

    public void LoadLevelgrid()
    {   
        InitializeSingleton();
        DestroyCells();
        grid = new CellArray2D(width, height);
        GridContainer = new GameObject("Grid Container");
        morphCellDict = new Dictionary <int, List<CellVisualizer>>();
        PopulateDictionary();
        ApplyVisualSettings();

        grid = gridSettings.grid;
        playerCell = gridSettings.playerCell;
        targetCell = gridSettings.targetCell;

        foreach (Cell cell in grid.cells)
        {   
            GameObject cellGO = SpawnItem("cell", cell.vector);
            BuildCell(cellGO, cell);
            cell.visualizer.TransformCell();
        }

        player = SpawnItem("player", playerCell);
        target = SpawnItem("target", targetCell);
    }

    public void Spawn()
    {   
        InitializeSingleton();
        DestroyCells();
        GridContainer = new GameObject("Grid Container");
        ApplyVisualSettings();
        PopulateDictionary();
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
                
                GameObject cellGO = SpawnItem("cell", cell.vector);
                BuildCell(cellGO, cell);
            }
        }
    }

    private void PopulateDictionary()
    {
        prefabDictionary.Clear();

        foreach (PrefabObject prefab in visualSettings.prefabList)
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
                OnReachedTarget?.Invoke();
            }

            if (GetCell(playerCell).isGlass)
            {
                GetCell(playerCell).DropCell();
            }

            player.GetComponent<PlayerMovement>().Assemble(new Vector3(dir.x, 0, dir.y)); 

            playerCell = newCell;

            if (GetCell(playerCell).isButton)
            {   
                ToggleCellSizes(GetCell(playerCell).morphIndex);
            }
        }
        return;
    }

    public void ToggleCellSizes(int tag)
    {
        if (morphCellDict != null && morphCellDict.ContainsKey(tag))
        {
            List<CellVisualizer> cells = morphCellDict[tag];

            foreach (CellVisualizer cell in cells)
            {   
                cell.ToggleSize();
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

    public void BuildCell(GameObject instance, Cell cell)
    {   
        CellVisualizer visualizer = instance.AddComponent<CellVisualizer>();
        visualizer.gridCell = cell;
        cell.visualizer = visualizer;
        cell.cellGameObject = instance;
    }

    public GameObject SpawnItem(string tag, Vector2 pos)
    {
        if (prefabDictionary.ContainsKey(tag))
        {
            PrefabObject prefab = prefabDictionary[tag];
            Vector3 position = new Vector3(pos.x * offsetX, prefab.height, pos.y * offsetY);
            GameObject instance = Instantiate(prefab.prefab, position, Quaternion.identity, transform);
            return instance;
        }
        else
        {
            Debug.LogWarning("Prefab with tag " + tag + " does not exist in the dictionary.");
            return null;
        }
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
        offsetX = visualSettings.offsetX;
        offsetY = visualSettings.offsetY;
        materials = visualSettings.materials;
    }
}
