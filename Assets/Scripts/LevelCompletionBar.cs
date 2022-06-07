using UnityEngine;
using UnityEngine.UI;

public class LevelCompletionBar : MonoBehaviour
{
    [SerializeField] private Image fillBar;

    [SerializeField] private Color levelNotCompletedColor = Color.white;
    [SerializeField] private Color levelCompletedBarColor = Color.yellow;
    [SerializeField] private Color levelFullyCompletedBarColor = Color.green;

    public void SetCompletionPercent(LevelData levelData)
    {
        Color color;
        if (levelData.IsFullyCompleted)
        {
            color = levelFullyCompletedBarColor;
        }
        else if (levelData.IsCompleted)
        {
            color = levelCompletedBarColor;
        }
        else
        {
            color = levelNotCompletedColor;
        }

        fillBar.color = color;
    }
}