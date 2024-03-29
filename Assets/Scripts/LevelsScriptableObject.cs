using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class LevelsScriptableObject : ScriptableObject {
    [System.Serializable]
    public class LevelInfo {
        public int levelID;
        public string levelTag;
    }

    public List<LevelInfo> levelList = new List<LevelInfo>();
    public Dictionary<int, string> LevelNameDict = new Dictionary<int, string>();

    public void PopulateDictionaryFromList() {
        LevelNameDict.Clear(); // Clear the dictionary first

        foreach (var levelInfo in levelList) {
            LevelNameDict[levelInfo.levelID] = levelInfo.levelTag;
        }
    }
}
