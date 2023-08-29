using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{   
    // public TextMeshProUGUI textMeshPro;

    // void Start()
    // {
    //     textMeshPro = GetComponent<TextMeshProUGUI>();
    //     textMeshPro.text = "lvl. " + GridManager.Instance.level;
    // }
    public GameObject menuUI;
    public GameObject settingsUI;

    public void ShowSettingsMenu()
    {
        if (menuUI != null)
            menuUI.SetActive(false);

        if (settingsUI != null)
            settingsUI.SetActive(true);
    }
}
