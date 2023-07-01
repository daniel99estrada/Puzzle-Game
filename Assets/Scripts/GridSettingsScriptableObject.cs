using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid Settings", menuName = "ScriptableObjects/Grid Settings")]
public class GridSettingsScriptableObject : ScriptableObject
{

    [Header("Grid Dimensions")]
    public int width;
    public int height;

    public CellArray2D grid;
    public Vector2 playerCell;
    public Vector2 targetCell;
    public int level;
    public string levelTag;

    // Method to save current settings to the dictionary
    public void SaveCurrentSettings(Grid _grid)
    {
        // Settings newSettings = new Settings();

        width = _grid.width;
        height = _grid.height;
        grid = _grid.grid;
        playerCell = _grid.playerCell;
        targetCell = _grid.targetCell;
        level = _grid.level;
        levelTag = _grid.levelTag;

        // // If the key already exists, update the settings
        // if (settingsDict.ContainsKey(key))
        // {
        //     settingsDict[key] = newSettings;
        // }
        // // Otherwise, add a new key-value pair
        // else
        // {
        //     settingsDict.Add(key, newSettings);
        // }
    }

    public void RetrieveSettings(Grid _grid)
    {
        // if (settingsDict.ContainsKey(key))
          
            _grid.width = width;
            _grid.height = height;
            _grid.grid = grid;
            _grid.playerCell = playerCell;
            _grid.targetCell = targetCell;
            _grid.level = level;
            _grid.levelTag = levelTag; 
    }
}
