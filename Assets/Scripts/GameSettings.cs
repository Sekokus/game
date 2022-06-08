using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Other/Game Settings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        private const string SettingsPath = "GameSettings";
        private static GameSettings _instance;

        public static GameSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<GameSettings>(SettingsPath);
                }

                return _instance;
            }
        }

        public float cameraShakeStrength;
        public float chromaticAberrationStrength;
        public float motionBlurStrength;
    }
}