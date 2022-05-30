using Player;
using UnityEngine;

namespace UI
{
    [DefaultExecutionOrder(2)]
    public abstract class ResourceBar : MonoBehaviour
    {
        private Resource _resource;

        private void Awake()
        {
            var playerFactory = Container.Get<PlayerFactory>();
            playerFactory.WhenPlayerAvailable(
                player =>
                {
                    _resource = GetResource(player);
                    _resource.OnValueChanged += OnValueChanged;
                });
        }

        private void OnDisable()
        {
            if (_resource != null)
            {
                _resource.OnValueChanged -= OnValueChanged;
            }
        }

        protected abstract Resource GetResource(PlayerCore player);

        protected abstract void OnValueChanged(Resource resource);
    }
}