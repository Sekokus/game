using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameSettingsInitializer : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;

        private void Awake()
        {
            GameSettings.SetInstance(gameSettings);
        }
    }
}