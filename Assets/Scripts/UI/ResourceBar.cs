using Sekokus;
using Sekokus.Player;
using UnityEngine;

namespace UI
{
    public abstract class ResourceBar : MonoBehaviour
    {
        private PlayerCore _player;

        private void Awake()
        {
            var playerFactory = Container.Get<PlayerFactory>();
            playerFactory.WhenPlayerAvailable(
                player =>
                {
                    _player = player;
                    Initialize();
                });
        }

        private void Initialize()
        {
            var resource = GetResource(_player);
            resource.OnValueChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            if (!_player)
            {
                return;
            }

            var resource = GetResource(_player);
            resource.OnValueChanged -= OnValueChanged;
        }

        protected abstract Resource GetResource(PlayerCore player);

        protected abstract void OnValueChanged(Resource resource);
    }
}