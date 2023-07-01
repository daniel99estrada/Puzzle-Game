using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "Grid Settings", menuName = "ScriptableObjects/Grid Settings")]
public class GridSettingsScriptableObject : ScriptableObject
{    
    [Header("Grid Dimensions")]
    public int width;
    public int height;

    [SerializeField]
    public CellArray2D grid;
    public Vector2 playerCell;
    public Vector2 targetCell;
    public int level;
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
    public string levelTag;

    public void SaveCameraSettings()
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            Camera cameraComponent = mainCamera.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                cameraPosition = mainCamera.transform.position;
                cameraRotation = mainCamera.transform.rotation.eulerAngles;
            }
        }
    }

    public void LoadCameraSettings()
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            Camera cameraComponent = mainCamera.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                mainCamera.transform.position = cameraPosition;
                mainCamera.transform.eulerAngles = cameraRotation;
            }
        }
        else
        {
            Debug.LogWarning("Main camera not found in the scene.");
        }
    }
    public void SaveCurrentSettings(Grid _grid)
    {   
        width = _grid.width;
        height = _grid.height;
        grid = _grid.grid;
        playerCell = _grid.playerCell;
        targetCell = _grid.targetCell;
        levelTag = _grid.levelTag;

        string filePath = Application.persistentDataPath + "/" + levelTag + ".json";
        SaveToFile(filePath);
    }

    public void SaveToFile(string filePath)
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(filePath, json);
    }

public static GridSettingsScriptableObject LoadFromFile(string filePath)
{
    if (File.Exists(filePath))
    {
        string json = File.ReadAllText(filePath);
        GridSettingsScriptableObject gridSettings = CreateInstance<GridSettingsScriptableObject>();
        JsonUtility.FromJsonOverwrite(json, gridSettings);
        return gridSettings;
    }
    else
    {
        Debug.LogWarning("Save file not found: " + filePath);
        return null;
    }
}

}