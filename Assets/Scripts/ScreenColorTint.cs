using UnityEngine;
using UnityEngine.UI;

public class ScreenColorTint : MonoBehaviour
{
    [SerializeField] private Image screenImage;

    public void Enable()
    {
        screenImage.enabled = true;
    }

    public void Disable()
    {
        screenImage.enabled = false;
    }
        
    public void SetColor(Color color)
    {
        screenImage.color = color;
    }

    public void SetAlpha(float alpha)
    {
        var color = screenImage.color;
        color.a = alpha;
        SetColor(color);
    }
}