using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellVisualizer : MonoBehaviour {
    
    private MeshRenderer meshRenderer;

    public Cell gridCell;
    public int morphIndex;
    public bool fullSize;
    public float buttonHeight = 0.5f;
    public bool morphable;

    private float smallHeight = 0.45f;
    private float normalHeight = 0f;
    private Vector3 smallSize = new Vector3(0.6f, 0.1f, 0.6f);
    private Vector3 normalSize = new Vector3(1f, 1f, 1f);

    
    public float lerpSpeed = 1f;

    void Start()
    {   
        meshRenderer = GetComponent<MeshRenderer>();
        Grid.Instance.grid[gridCell.x, gridCell.y] = gridCell;
        
        if (morphable) 
        {
            Grid.Instance.AddToMorphableList(morphIndex, this);
        }
    }

    public void Enabled(bool enabled) {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = enabled;
        gridCell.isEnabled = enabled;
    }

    public void IsGlass(bool enabled) {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = Grid.Instance.glassMaterial;
        gridCell.isGlass = enabled;
    }

    public void IsButton()
    {   
        Vector2 pos = new Vector2(gridCell.x, gridCell.y);
        GameObject button = Grid.Instance.SpawnItem("button", pos);
        gridCell.isButton = true;
        gridCell.morphIndex = morphIndex;
        button.GetComponent<MeshRenderer>().material = Grid.Instance.materials[morphIndex];
    }

    public void IsMorhpable()
    {   
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = Grid.Instance.materials[morphIndex];   
        morphable = true; 
        StartCoroutine(LerpSizeAndPosition());
    }

    public IEnumerator LerpSizeAndPosition()
    {   
        gridCell.isEnabled = !gridCell.isEnabled;

        fullSize = gridCell.isEnabled;

        Vector3 startSize = fullSize ? smallSize : normalSize;x
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
        StartCoroutine(LerpSizeAndPosition());
    }
    public void ActivateGravity()
    {
        this.gameObject.AddComponent<Rigidbody>();
    }
}