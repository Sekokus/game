using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Group", fileName = "LevelGroup", order = 1)]
public class LevelGroup : ScriptableObject
{
    [SerializeField] private List<LevelData> levelDatas;

    public IReadOnlyList<LevelData> LevelDatas => levelDatas;

    public void AddLevelData(LevelData levelData)
    {
        levelDatas.Add(levelData);
        RevalidateData();
    }

    public void RemoveLevelData(LevelData levelData)
    {
        levelDatas.Remove(levelData);
        RevalidateData();
    }

    [ContextMenu(nameof(ResetPlayerDataInGroup))]
    public void ResetPlayerDataInGroup()
    {
        foreach (var levelData in levelDatas)
        {
            levelData.ResetPlayerData();
        }

        Debug.Log("All group data resetted");
    }

    private void OnValidate()
    {
        RevalidateData();
    }

    private void RevalidateData()
    {
        levelDatas.RemoveAll(ld => !ld);

        for (var i = 0; i < levelDatas.Count; i++)
        {
            levelDatas[i].nextLevel = i + 1 < levelDatas.Count ? levelDatas[i + 1] : null;
            levelDatas[i].levelGroup = this;
        }
    }
}