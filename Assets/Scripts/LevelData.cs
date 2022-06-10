using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Data", fileName = "LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    public LevelGroup levelGroup;
    
    public string levelName;
    public string sceneName;

    public int bestLevelTimeMs;
    public int bestLevelCoinCount;

    public int requiredCount; 
    public int maxCount;
    public CountType countType;

    public Sprite previewSprite;
    
    public bool IsCompleted => bestLevelCoinCount >= requiredCount;
    public bool IsFullyCompleted => bestLevelCoinCount == maxCount;
    
    public float CompletionPercent => (float)bestLevelCoinCount / maxCount;

    public LevelData nextLevel;

    [ContextMenu(nameof(ResetPlayerData))]
    public void ResetPlayerData()
    {
        bestLevelCoinCount = 0;
        bestLevelTimeMs = 0;

        Debug.Log("Level data resetted");
    }
}