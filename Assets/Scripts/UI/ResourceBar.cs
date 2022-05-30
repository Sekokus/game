using Player;
using UnityEngine;

namespace UI
{
    public abstract class ResourceBar : MonoBehaviour
    {
        private Resource _resource;
        private PlayerFactory _playerFactory;

        private void Awake()
        {
            _playerFactory = Container.Get<PlayerFactory>();
            _playerFactory.WhenPlayerAvailable(OnPlayerCreated);
        }

        private void OnPlayerCreated(PlayerCore player)
        {
            _resource = GetResource(player);
            _resource.OnValueChanged += OnValueChanged;
        }

        private void OnDestroy()
        {
            if (_resource != null)
            {
                _resource.OnValueChanged -= OnValueChanged;
            }

            if (_playerFactory != null)
            {
                _playerFactory.PlayerCreated -= OnPlayerCreated;
            }
        }

        protected abstract Resource GetResource(PlayerCore player);

        protected abstract void OnValueChanged(Resource resource);
    }
}