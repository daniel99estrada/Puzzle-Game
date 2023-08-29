// LevelButton.cs
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int level;

    public void OpenLevel()
    {
        Debug.Log("Opening level: " + level);
    }
}
