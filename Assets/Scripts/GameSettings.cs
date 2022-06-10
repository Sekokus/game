using System;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Other/Game Settings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        private const float ValueChangedEpsilon = 1e-5f;

        public static GameSettings Instance { get; private set; }

        public float CameraShakeStrength
        {
            get => cameraShakeStrength;
            set
            {
                if (Math.Abs(cameraShakeStrength - value) < ValueChangedEpsilon)
                {
                    return;
                }

                cameraShakeStrength = value;
                PropertyValueChanged?.Invoke(nameof(CameraShakeStrength));
            }
        }

        public float ChromaticAberrationStrength
        {
            get => chromaticAberrationStrength;
            set
            {
                if (Math.Abs(chromaticAberrationStrength - value) < ValueChangedEpsilon)
                {
                    return;
                }

                chromaticAberrationStrength = value;
                PropertyValueChanged?.Invoke(nameof(ChromaticAberrationStrength));
            }
        }

        public float MotionBlurStrength
        {
            get => motionBlurStrength;
            set
            {
                if (Math.Abs(motionBlurStrength - value) < ValueChangedEpsilon)
                {
                    return;
                }

                motionBlurStrength = value;
                PropertyValueChanged?.Invoke(nameof(MotionBlurStrength));
            }
        }

        public bool BloodEnabled
        {
            get => bloodEnabled;
            set
            {
                if (bloodEnabled == value)
                {
                    return;
                }

                bloodEnabled = value;
                PropertyValueChanged?.Invoke(nameof(BloodEnabled));
            }
        }

        public event Action<string> PropertyValueChanged;

        [SerializeField] private float cameraShakeStrength;
        [SerializeField] private float chromaticAberrationStrength;
        [SerializeField] private float motionBlurStrength;
        [SerializeField] private bool bloodEnabled;

        public static void SetInstance(GameSettings gameSettings)
        {
            Instance = gameSettings;
        }
    }
}