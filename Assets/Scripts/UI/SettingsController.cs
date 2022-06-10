using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsController : MonoBehaviour
    {
        private const int ReferenceValue = 20;

        [SerializeField] private Slider cameraShakeSlider;
        [SerializeField] private Slider chromaticAberrationSlider;
        [SerializeField] private Slider motionBlurSlider;
        [SerializeField] private Toggle bloodToggle;

        private void Awake()
        {
            InitializeComponentValues();
            SubscribeToComponentEvents();
        }

        private void InitializeComponentValues()
        {
            cameraShakeSlider.SetValueWithoutNotify(GameSettings.Instance.CameraShakeStrength * ReferenceValue);
            chromaticAberrationSlider.SetValueWithoutNotify(GameSettings.Instance.ChromaticAberrationStrength *
                                                            ReferenceValue);
            motionBlurSlider.SetValueWithoutNotify(GameSettings.Instance.MotionBlurStrength * ReferenceValue);
            bloodToggle.SetIsOnWithoutNotify(GameSettings.Instance.BloodEnabled);
        }

        private void SubscribeToComponentEvents()
        {
            cameraShakeSlider.onValueChanged.AddListener(value =>
            {
                GameSettings.Instance.CameraShakeStrength = value / ReferenceValue;
            });

            chromaticAberrationSlider.onValueChanged.AddListener(value =>
            {
                GameSettings.Instance.ChromaticAberrationStrength = value / ReferenceValue;
            });

            motionBlurSlider.onValueChanged.AddListener(value =>
            {
                GameSettings.Instance.MotionBlurStrength = value / ReferenceValue;
            });

            bloodToggle.onValueChanged.AddListener(value => { GameSettings.Instance.BloodEnabled = value; });
        }
    }
}