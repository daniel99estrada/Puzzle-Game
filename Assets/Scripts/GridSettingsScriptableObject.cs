using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid Settings", menuName = "ScriptableObjects/Grid Settings")]
public class GridSettingsScriptableObject : ScriptableObject
{    
    [SerializeField]
    public CellArray2D grid;
    public Dictionary <int, List<CellVisualizer>> morphCellDict = new Dictionary <int, List<CellVisualizer>>();
    public Vector2 playerCell;
    public Vector2 targetCell;
    public int level;
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;

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
        grid = _grid.grid;
        morphCellDict = _grid.morphCellDict;
        playerCell = _grid.playerCell;
        targetCell = _grid.targetCell;
        SaveCameraSettings();
    }
}