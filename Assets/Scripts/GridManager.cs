using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GridManager : MonoBehaviour
{
    public LevelsScriptableObject levelDatabase;

    [Header("Cell Dictionary")]
    public Dictionary <int, List<CellVisualizer>> morphCellDict;

    public static GridManager Instance;
    public int level;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ListFilesInPersistentDataPath();
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

    private void ListFilesInPersistentDataPath()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath);

        if (files.Length > 0)
        {
            Debug.Log("Files in Application.persistentDataPath:");
            foreach (string file in files)
            {
                Debug.Log(file);
            }
        }
        else
        {
            Debug.Log("No files found in Application.persistentDataPath.");
        }
    }
    public string GetLevelTag() {

        // Check if the level ID is in the dictionary
        if (levelDatabase.LevelNameDict.ContainsKey(level)) {
            // Return the level name
            return levelDatabase.LevelNameDict[level];
        } else {
            // The level ID is not in the dictionary, so return null
            return null;
        }
    }
}
