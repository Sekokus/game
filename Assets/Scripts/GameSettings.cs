using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Other/Game Settings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public static GameSettings Instance { get; private set; }

        public float cameraShakeStrength;
        public float chromaticAberrationStrength;
        public float motionBlurStrength;

        private void OnEnable()
        {
            Instance = this;
        }
    }
}