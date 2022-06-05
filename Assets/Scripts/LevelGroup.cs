using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Group", fileName = "LevelGroup", order = 1)]
public class LevelGroup : ScriptableObject
{
    public List<LevelData> levelDatas;

    [ContextMenu(nameof(ResetPlayerDataInGroup))]
    public void ResetPlayerDataInGroup()
    {
        foreach (var levelData in levelDatas)
        {
            levelData.ResetPlayerData();
        }
        
        Debug.Log("All group data resetted");
    }
}