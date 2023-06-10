using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid Settings", menuName = "ScriptableObjects/Grid Settings")]
public class GridSettingsScriptableObject : MonoBehaviour
{    
    [SerializeField]
    public CellArray2D grid;
    public Dictionary <int, List<CellVisualizer>> morphCellDict = new Dictionary <int, List<CellVisualizer>>();
    public Vector2 playerCell;
    public Vector2 targetCell;
    public int level;
}