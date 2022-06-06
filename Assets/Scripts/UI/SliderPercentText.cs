using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderPercentText : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private float oneHundredPercentValue = 1;

        private void Awake()
        {
            slider.onValueChanged.AddListener(UpdateText);
            UpdateText(slider.value);
        }

        private void UpdateText(float value)
        {
            var percents = value * 100 / oneHundredPercentValue;
            textMesh.text = percents + "%";
        }
    }
}