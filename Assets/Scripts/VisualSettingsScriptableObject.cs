using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "VisualSettingsScriptableObject", menuName = "ScriptableObjects/VisualSettings")]
public class VisualSettingsScriptableObject : ScriptableObject
{
    public float scale = 1f;
    public GameObject playerPrefab;
    public GameObject targetPrefab;
    public GameObject prefab;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public float playerHeight;
    public float itemHeight;
    public List<Material> materials;
    [SerializeField]
    public CellArray2D grid;
    public Dictionary <int, List<CellVisualizer>> morphCellDict = new Dictionary <int, List<CellVisualizer>>();
    public Vector2 playerCell;
    public Vector2 targetCell;
}