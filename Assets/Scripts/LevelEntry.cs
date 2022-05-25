using UnityEngine;

namespace Sekokus
{
    public class LevelEntry : MonoBehaviour
    {
        [SerializeField]
        private LevelStartCountdown countdown;

        private void Awake()
        {     
            countdown.CountdownEnded += OnCountdownEnded;
        }

        public void StartLevel()
        {
            Time.timeScale = 0;
            countdown.AllowCountdownStart();
        }

        private void OnCountdownEnded()
        {
            Time.timeScale = 1;
        }
    }
}