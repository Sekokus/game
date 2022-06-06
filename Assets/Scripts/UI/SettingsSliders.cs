using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsSliders : MonoBehaviour
    {
        private const int ReferenceValue = 20;

        [SerializeField] private Slider cameraShakeSlider;
        [SerializeField] private Slider chromaticAberrationSlider;
        [SerializeField] private Slider motionBlurSlider;

        private void Awake()
        {
            InitializeSliderValues();
            SubscribeToSliderEvents();
        }

        private void InitializeSliderValues()
        {
            cameraShakeSlider.SetValueWithoutNotify(GameSettings.Instance.cameraShakeStrength * ReferenceValue);
            chromaticAberrationSlider.SetValueWithoutNotify(GameSettings.Instance.chromaticAberrationStrength *
                                                            ReferenceValue);
            motionBlurSlider.SetValueWithoutNotify(GameSettings.Instance.motionBlurStrength * ReferenceValue);
        }

        private void SubscribeToSliderEvents()
        {
            cameraShakeSlider.onValueChanged.AddListener(value =>
            {
                GameSettings.Instance.cameraShakeStrength = value / ReferenceValue;
            });

            chromaticAberrationSlider.onValueChanged.AddListener(value =>
            {
                GameSettings.Instance.chromaticAberrationStrength = value / ReferenceValue;
            });

            motionBlurSlider.onValueChanged.AddListener(value =>
            {
                GameSettings.Instance.motionBlurStrength = value / ReferenceValue;
            });
        }
    }
}