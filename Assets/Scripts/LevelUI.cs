using UnityEngine;
using UnityEngine.UIElements;

public class LevelUI : MonoBehaviour
{
    private int currentLevel;
    public UIDocument uiDocument;
    public Label levelLabel;

    private void Start()
    {   
        currentLevel = GridManager.Instance.level;
        // Find the level label in the UIDocument
        levelLabel = uiDocument.rootVisualElement.Q<Label>("LevelLabel");

        // Update the initial level text
        UpdateLevelText();

        // Find the restart button in the UIDocument
        Button restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");

        // Add a click event handler to the restart button
        restartButton.clicked += RestartLevel;

        Grid.Instance.OnReachedTarget += DisableUI;
    }

    private void UpdateLevelText()
    {
        // Update the level text with the currentLevel value
        levelLabel.text = "Level " + currentLevel;
    }

    private void RestartLevel()
    {
        // Restart the current level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void EnableUI()
    {
        // Clear the UI before loading the new screen
        uiDocument.enabled = true;

    }

    private void DisableUI()
    {
        // Clear the root visual element of the UIDocument to remove all UI elements
        uiDocument.enabled = false;
    }
}
