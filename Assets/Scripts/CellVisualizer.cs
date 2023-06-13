using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellVisualizer : MonoBehaviour {
    
    private MeshRenderer meshRenderer;

    public Cell gridCell;
    public GameObject button;
    

    private float smallHeight = 0.45f;
    private float normalHeight = 0f;
    private Vector3 smallSize = new Vector3(0.6f, 0.1f, 0.6f);
    private Vector3 normalSize = new Vector3(1f, 1f, 1f);

    
    private float lerpSpeed = 2f;

    public int morphIndex;

    public void SaveCell()
    {   
        gridCell.renderer = GetComponent<MeshRenderer>();

        Transform sourceTransform = this.gameObject.GetComponent<Transform>();
        gridCell.position = sourceTransform.position;
        gridCell.scale = sourceTransform.localScale;
        
        Grid.Instance.grid[gridCell.x, gridCell.y] = gridCell;
        
        if (gridCell.isMorphable) 
        {
            Grid.Instance.AddToMorphableList(morphIndex, this);
        }
    }

    public void LoadSettings()
    {   
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer = gridCell.renderer;

        transform.position = gridCell.position;
        transform.localScale = gridCell.scale;
    }

    public void Enabled(bool enabled) {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = enabled;
        gridCell.isEnabled = enabled;
    }

    public void IsGlass(bool enabled) {
        meshRenderer = GetComponent<MeshRenderer>();
        if (enabled)
        {
            meshRenderer.material = Grid.Instance.visualSettings.glassMaterial;
        }
        else
        {
            meshRenderer.material = Grid.Instance.visualSettings.cellMaterial;
        }
        
        gridCell.isGlass = enabled;
    }

    public void TransformCell()
    {
        if (gridCell.isGlass)
        {
            IsGlass(gridCell.isGlass);
        }

        if (gridCell.isButton)
        {   
            morphIndex = gridCell.morphIndex;
            IsButton(gridCell.isButton);
        }
        
        if (gridCell.isMorphable)
        {   
            morphIndex = gridCell.morphIndex;
            Grid.Instance.AddToMorphableList(gridCell.morphIndex, this);
            IsMorphable(gridCell.isMorphable);
        }
        else
        {
            Enabled(gridCell.isEnabled);
        }
    }

    public void IsButton(bool enabled)
    {                   
        if (enabled)
        {   
            Vector2 pos = new Vector2(gridCell.x, gridCell.y);
            button = Grid.Instance.SpawnItem("button", pos);
            button.transform.SetParent(transform); 
            
            gridCell.morphIndex = morphIndex;
            button.GetComponent<MeshRenderer>().material = Grid.Instance.visualSettings.materials[morphIndex];
        }
        else if (button != null)
        {
            Destroy(button);
        }

        gridCell.isButton = enabled;
    }

    public void IsMorphable(bool enabled)
    {   
        gridCell.isMorphable = enabled;

        if (enabled)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = Grid.Instance.visualSettings.materials[morphIndex];   
            gridCell.morphIndex = morphIndex;
        
            StartCoroutine(LerpSizeAndPosition());
        }
        else
        {
            gridCell.isEnabled = true;
            StartCoroutine(LerpSizeAndPosition());
            meshRenderer.material = Grid.Instance.visualSettings.cellMaterial;
        }

        
    }

    public IEnumerator LerpSizeAndPosition()
    {   
        bool fullSize = gridCell.isEnabled;

        Vector3 startSize = fullSize ? smallSize : normalSize;
        Vector3 endSize = fullSize ? normalSize : smallSize;

        float startY = fullSize ? smallHeight : normalHeight;
        float endY = fullSize ? normalHeight : smallHeight;


        float time = 0f;
        while (time < 1f)
        {
            float currentY = Mathf.Lerp(startY, endY, time);

            Vector3 startPosition = new Vector3(transform.localPosition.x, currentY, transform.localPosition.z);
            Vector3 endPosition = new Vector3(transform.localPosition.x, currentY, transform.localPosition.z);

            transform.localScale = Vector3.Lerp(startSize, endSize, time);
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, time);

            time += lerpSpeed * Time.deltaTime;

            yield return null;
        }

        transform.localScale = endSize;
        transform.localPosition = new Vector3(transform.localPosition.x, endY, transform.localPosition.z);
    }

    public void ToggleSize()
    {   
        gridCell.isEnabled = !gridCell.isEnabled;
        StartCoroutine(LerpSizeAndPosition());
    }

    public void ActivateGravity()
    {
        this.gameObject.AddComponent<Rigidbody>();
    }

}