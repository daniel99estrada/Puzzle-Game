// LevelButtonsPopulator.cs
using UnityEngine;

public class LevelButtonsPopulator : MonoBehaviour
{
    public GameObject buttonPrefab;
    public int numberOfButtons = 10;

    private void Start()
    {
        // Populate the UI parent with buttons
        PopulateButtons();
    }

    private void PopulateButtons()
    {
        // Check if the button prefab is assigned
        if (buttonPrefab == null)
        {
            Debug.LogError("Button Prefab is not assigned in the inspector!");
            return;
        }

        // Instantiate buttons and set their properties
        for (int i = 1; i <= numberOfButtons; i++)
        {
            GameObject buttonGO = Instantiate(buttonPrefab, transform);
            buttonGO.name = "LevelButton_" + i;

            // Add the LevelButton script component and set the level variable
            LevelButton levelButton = buttonGO.GetComponent<LevelButton>();
            if (levelButton != null)
            {
                levelButton.level = i;
            }
            else
            {
                Debug.LogError("LevelButton script component not found on the prefab!");
            }

            // Add a Text component to the button to show the level number (optional)
            // Text buttonText = buttonGO.GetComponentInChildren<Text>();
            // if (buttonText != null)
            // {
            //     buttonText.text = "Level " + i;
            // }
        }
    }
}
