using UnityEngine;
using UnityEngine.UI;

public class LevelCompletionBar : MonoBehaviour
{
    [SerializeField] private Image fillBar;

    [SerializeField] private Color levelCompletedBarColor = Color.yellow;
    [SerializeField] private Color levelFullyCompletedBarColor = Color.green;

    public void SetCompletionPercent(float percent)
    {
        fillBar.fillAmount = percent;

        var color = percent < 1 ? levelCompletedBarColor : levelFullyCompletedBarColor;
        fillBar.color = color;
    }
}