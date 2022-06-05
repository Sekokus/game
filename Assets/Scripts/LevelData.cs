using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Data", fileName = "LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public string sceneName;

    public int bestLevelTimeMs;
    public int bestLevelCoinCount;

    public int requiredCoinCount;
    public int maxCoinCount;

    public Sprite previewSprite;
    
    public bool IsCompleted => bestLevelCoinCount >= requiredCoinCount;
    public bool IsFullyCompleted => bestLevelCoinCount == maxCoinCount;
    
    public float CompletionPercent => (float)bestLevelCoinCount / maxCoinCount;

    public LevelData nextLevel;

    [ContextMenu(nameof(ResetPlayerData))]
    public void ResetPlayerData()
    {
        bestLevelCoinCount = 0;
        bestLevelTimeMs = 0;

        Debug.Log("Level data resetted");
    }
}