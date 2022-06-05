using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Group", fileName = "LevelGroup", order = 1)]
public class LevelGroup : ScriptableObject
{
    [SerializeField] private LevelData[] levelDatas;

    public IReadOnlyList<LevelData> GetLevelDatas()
    {
        return levelDatas;
    }

    [ContextMenu(nameof(ResetPlayerDataInGroup))]
    private void ResetPlayerDataInGroup()
    {
        foreach (var levelData in levelDatas)
        {
            levelData.ResetPlayerData();
        }
        
        Debug.Log("All group data resetted");
    }
}