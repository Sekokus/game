using Sekokus.Player;
using UnityEngine;

namespace Sekokus
{
    [DefaultExecutionOrder(-1)]
    public class LevelBootstrap : MonoBehaviour
    {
        [SerializeField]
        private LevelStartCountdown countdown;

        private LevelFactory _levelFactory;

        private PlayerMarker _playerMarker;
        
        private void Start()
        {
            Time.timeScale = 0;
            countdown.CountdownEnded += OnCountdownEnded;
            
            _levelFactory = Container.Get<LevelFactory>();
            
            FindMarkers();
            _levelFactory.CreateLevel(_playerMarker);
        }

        private void OnCountdownEnded()
        {
            Time.timeScale = 1;
        }

        private void FindMarkers()
        {
            _playerMarker = FindObjectOfType<PlayerMarker>();
        }
    }
}