using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

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
}
