using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public class PlayerFactory
    {
        public event Action<PlayerCore> PlayerCreated;

        private const string PlayerPath = "Player";

        private static PlayerCore _playerCorePrefab;
        private PlayerCore _player;

        public PlayerFactory()
        {
            LoadResources();
        }

        public PlayerCore CreatePlayerFromMarker(Marker marker)
        {
            _player = Object.Instantiate(_playerCorePrefab, marker.Location, marker.Rotation);
            PlayerCreated?.Invoke(_player);
            return _player;
        }

        public void WhenPlayerAvailable(Action<PlayerCore> action)
        {
            if (_player)
            {
                action(_player);
            }
            else
            {
                PlayerCreated += action;
            }
        }

        private static void LoadResources()
        {
            _playerCorePrefab ??= Resources.Load<PlayerCore>(PlayerPath);
        }
    }
}