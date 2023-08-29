using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class LevelsScriptableObject : ScriptableObject {

    public Dictionary<int, string> Levels = new Dictionary<int, string>();

    public List<LevelData> LevelsList = new List<LevelData>();

    public class LevelData {
        public int id;
        public string name;

        public LevelData(int id, string name) {
            this.id = id;
            this.name = name;
        }
    }

    public void PopulateLevels() {
        foreach (LevelData level in LevelsList) {
            Levels.Add(level.id, level.name);
        }
    }

}
