using Sekokus;
using UnityEngine;

namespace UI
{
    [DefaultExecutionOrder(2)]
    public abstract class ResourceBar : MonoBehaviour
    {
        private void OnEnable()
        {
            var player = PlayerManager.Instance.Player;
            if (player == null)
            {
                return;
            }

            var resource = GetResource(player);
            resource.OnValueChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            var player = PlayerManager.Instance.Player;
            if (player == null)
            {
                return;
            }

            var resource = GetResource(player);
            resource.OnValueChanged -= OnValueChanged;
        }

        protected abstract CharacterResource GetResource(PlayerCore player);

        protected abstract void OnValueChanged(CharacterResource resource);
    }
}
