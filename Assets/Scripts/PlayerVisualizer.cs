using UnityEngine;

public class PlayerVisualizer : MonoBehaviour {
    public IntVector2 gridPosition;
    public float offsetX;
    public float offsetY;

    private void Awake() {
        GridVisualizer gridVisualizer = GridVisualizer.Instance;
        if (gridVisualizer != null) {
            offsetX = gridVisualizer.offsetX;
            offsetY = gridVisualizer.offsetY;
        }
    }

    public void MovePlayer(IntVector2 newPosition) {
        Vector3 position = new Vector3(newPosition.x * offsetX, newPosition.y * offsetY, transform.position.z);
        transform.position = position;
        gridPosition = newPosition;
    }
}
