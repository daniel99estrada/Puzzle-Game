using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid : MonoBehaviour
{
    [Header("Settings")]
    public VisualSettingsScriptableObject visualSettings;
    [SerializeField]
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

    public event Action OnReachedTarget;

    public static Grid Instance { get; private set; }
    
    private void Awake()
    {   
        InitializeSingleton();
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

    void DestroyGridContainer()
    {
        GameObject gridContainer = GameObject.Find("Grid Container"); // Find the "Grid Container" GameObject by name

        if (gridContainer != null) // Check if the "Grid Container" GameObject exists
        {
#if UNITY_EDITOR
            DestroyImmediate(gridContainer); // Destroy the "Grid Container" GameObject immediately in the editor
#else
            Destroy(gridContainer); // Destroy the "Grid Container" GameObject normally outside of the editor
#endif
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
        DestroyGridContainer();
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

    public void Spawn()
    {   
        InitializeSingleton();
        DestroyGridContainer();
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
            GameObject instance = Instantiate(prefab.prefab, position, Quaternion.identity, GridContainer.transform);
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
    if (morphCellDict == null)
        morphCellDict = new Dictionary<int, List<CellVisualizer>>();

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
        // Applying visual settings to the grid
        // scale = visualSettings.scale;
        offsetX = visualSettings.offsetX;
        offsetY = visualSettings.offsetY;
        materials = visualSettings.materials;
    }


}