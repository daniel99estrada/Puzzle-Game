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
}
