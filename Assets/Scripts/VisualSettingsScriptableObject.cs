using UnityEngine;

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
}