using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Grid : MonoBehaviour
{
    [Header("Settings")]
    public VisualSettingsScriptableObject visualSettings;
    [SerializeField]
    public GridSettingsScriptableObject gridSettings;
    public string gridTag;
    public int level;

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

    [Header("Grid Container")]
    public GameObject GridContainer;
    public float cameraOffset;
    public float gridOffset;

    [Header("Grid")]
    public CellArray2D grid;

    [Header("Player and Target GameObjects")]
    private GameObject player;
    private GameObject target;

    [Header("Prefab Dictionary")]
    public Dictionary<string, PrefabObject> prefabDictionary = new Dictionary<string, PrefabObject>();

    public event Action OnReachedTarget;

    public static Grid Instance { get; private set; }
    
    private void Start()
    {   
        LoadLevelgrid();
    }

    void SpawnGridContainer()
    {
        DestroyObject(GridContainer);
        GridContainer = new GameObject("Grid Container");
        GridContainer.transform.SetParent(transform);
    }

    public void SaveLevelgrid()
    {   
        foreach (Cell cell in grid.cells)
        {   
            cell.visualizer.SaveCell();
        }

        gridSettings.SaveCurrentSettings(this);
    }

    public void LoadLevelgrid()
    {   
        SpawnGridContainer();
        ApplyVisualSettings();
        PopulateDictionary();
        DestroyObject(player);

        string filePath = Application.persistentDataPath + "/" + level + "" + gridTag + ".json";
        gridSettings = GridSettingsScriptableObject.LoadFromFile(filePath);

        grid = gridSettings.grid;

        playerCell = gridSettings.playerCell;
        targetCell = gridSettings.targetCell;
        width = gridSettings.width;
        height = gridSettings.height;
        cameraOffset = gridSettings.cameraOffset;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {   
                Cell cell = gridSettings.grid[x, y];
                grid[x, y] = cell;
                
                GameObject cellGO = SpawnItem("cell", cell.vector);
                BuildCell(cellGO, cell);
                cell.visualizer.TransformCell();
            }
        }

        player = SpawnItem("player", playerCell);
        player.GetComponent<PlayerMovement>().Grid = this;
        target = SpawnItem("target", targetCell);

        Camera cameraComponent = GetComponentInChildren<Camera>();
        cameraComponent.orthographicSize = cameraOffset;
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
        SpawnGridContainer();
        ApplyVisualSettings();
        PopulateDictionary();
        CreateGrid();
        player = SpawnItem("player", playerCell);
        player.GetComponent<PlayerMovement>().Grid = this;
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
                player.GetComponent<PlayerMovement>().DisableMovement();
            }

            if (GetCell(playerCell).isGlass)
            {
                GetCell(playerCell).DropCell();
            }

            if (GetCell(playerCell).isSpike)
            {
                Debug.Log("You Lost");
            }

            if (GetCell(playerCell).isButton)
            {   
                GridManager.Instance.ToggleCellSizes(GetCell(playerCell).morphIndex);
            }

            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Assemble(new Vector3(dir.x, 0, dir.y));
            }

            playerCell = newCell;
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
        visualizer.Grid = this;
    }

    public GameObject SpawnItem(string tag, Vector2 pos)
    {
        if (prefabDictionary.ContainsKey(tag))
        {
            PrefabObject prefab = prefabDictionary[tag];
            Vector3 position = new Vector3(gridOffset + pos.x * offsetX, prefab.height, pos.y * offsetY);
            GameObject instance = Instantiate(prefab.prefab, position, Quaternion.identity, GridContainer.transform);
            return instance;
        }
        else
        {
            Debug.LogWarning("Prefab with tag " + tag + " does not exist in the dictionary.");
            return null;
        }
    }

    public void ApplyVisualSettings()
    {
        offsetX = visualSettings.offsetX;
        offsetY = visualSettings.offsetY;
        materials = visualSettings.materials;
    }

    void DestroyAllChildren(GameObject parentObject)
    {
        int childCount = parentObject.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform childTransform = parentObject.transform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            DestroyObject(childObject);
        }
    }

    public void DestroyObject(GameObject obj)
    {
        if (obj != null)
        {
    #if UNITY_EDITOR
            DestroyImmediate(obj);
    #else
            Destroy(obj);
    #endif 
        }    
    }
}