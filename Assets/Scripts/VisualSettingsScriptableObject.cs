using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "VisualSettingsScriptableObject", menuName = "ScriptableObjects/VisualSettings")]
public class VisualSettingsScriptableObject : ScriptableObject
{
    public float scale = 1f;
    public List<PrefabObject> prefabList = new List<PrefabObject>();
    public float offsetX = 1f;
    public float offsetY = 1f;
    public float playerHeight;
    public float itemHeight;
    public List<Material> materials;
    public Material glassMaterial;
}