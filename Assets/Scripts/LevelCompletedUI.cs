using UnityEngine;
using UnityEngine.UIElements;
using System;

public class LevelCompletedUI : MonoBehaviour
{
    public UIDocument uiDocument;
    public Button restartButton;
    public Button playButton;
    public Button homeButton;

    private void Start()
    {   
        // Assign the UIDocument component
        uiDocument = GetComponent<UIDocument>();

        // Find the buttons in the UIDocument
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        playButton = uiDocument.rootVisualElement.Q<Button>("PlayButton");
        homeButton = uiDocument.rootVisualElement.Q<Button>("HomeButton");

        // Subscribe to the OnReachedTarget event from the Grid singleton
        Grid.Instance.OnReachedTarget += EnableUI;

        // Add click event handlers to the buttons
        restartButton.clicked += RestartLevel;
        playButton.clicked += PlayNextLevel;
        homeButton.clicked += OpenMenu;
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

    private void RestartLevel()
    {
        // Restart the current level
        Debug.Log("Restarting level...");
        // Add your restart level logic here
    }

    private void PlayNextLevel()
    {
        // Play the next level
        Debug.Log("Playing next level...");
        // Add your play next level logic here
    }

    private void OpenMenu()
    {
        // Open the menu
        Debug.Log("Opening menu...");
        // Add your menu opening logic here
    }
}
