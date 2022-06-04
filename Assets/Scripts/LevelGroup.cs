using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Group", fileName = "LevelGroup", order = 1)]
public class LevelGroup : ScriptableObject
{
    [SerializeField] private LevelData[] levelDatas;

    public LevelGroupInfo GetInfo()
    {
        var isAllBeaten = levelDatas.All(ld => ld.bestLevelCoinCount >= ld.requiredCoinCount);
        var percentSum = levelDatas.Average(ld => (float)ld.bestLevelCoinCount / ld.maxCoinCount);
        return new LevelGroupInfo(isAllBeaten, percentSum);
    }

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