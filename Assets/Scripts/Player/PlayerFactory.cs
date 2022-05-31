using System;
using UnityEngine;
using UnityEngine.Animations;
using Object = UnityEngine.Object;

namespace Player
{
    public class PlayerFactory
    {
        public event Action<PlayerCore> PlayerCreated;

        private const string PlayerPath = "Player";
        private const string PlayerLookDirectionIndicatorPath = "Player_LookDirectionIndicator";

        private static PlayerCore _playerCorePrefab;
        private static PositionConstraint _lookDirectionIndicatorPrefab;

        private PlayerCore _player;

        public PlayerFactory()
        {
            LoadResources();
        }

        public PlayerCore CreatePlayerFromMarker(Marker marker)
        {
            _player = Object.Instantiate(_playerCorePrefab, marker.Location, marker.Rotation);
            
            var lookDirectionIndicator = Object.Instantiate(_lookDirectionIndicatorPrefab);
            lookDirectionIndicator.AddSource(new ConstraintSource
            {
                sourceTransform = _player.Transform,
                weight = 1
            });
            lookDirectionIndicator.constraintActive = true;

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
            if (_playerCorePrefab == null)
            {
                _playerCorePrefab = Resources.Load<PlayerCore>(PlayerPath);
            }

            if (_lookDirectionIndicatorPrefab == null)
            {
                _lookDirectionIndicatorPrefab = Resources.Load<PositionConstraint>(PlayerLookDirectionIndicatorPath);
            }
        }
    }
}