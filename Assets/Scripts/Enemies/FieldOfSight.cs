using System;
using Player;
using UnityEngine;

namespace Enemies
{
    public interface ISwitchable
    {
        void SwitchEnable();
        void SwitchDisable();
    }
    
    public class FieldOfSight : MonoBehaviour
    {
        [SerializeField] private float detectRadius;
        [SerializeField] private float scanInterval = 0.3f;

        private ISwitchable[] _switchables;

        private PlayerCore _player;
        private void Awake()
        {
            _switchables = GetComponentsInChildren<ISwitchable>();
            
            var playerFactory = Container.Get<PlayerFactory>();
            playerFactory.WhenPlayerAvailable(p => _player = p);

            SetControlledBehavioursState(false);
            
            InvokeRepeating(nameof(ScanForPlayer), 0, scanInterval);
        }

        private void ScanForPlayer()
        {
            if (!_player)
            {
                return;
            }

            var distance = Vector3.Distance(_player.Transform.position, transform.position);
            var detected = distance <= detectRadius;
            SetDetectedState(detected);
        }

        private bool _detected = false;

        private void SetDetectedState(bool detected)
        {
            if (_detected == detected)
            {
                return;
            }

            SetControlledBehavioursState(detected);
            _detected = detected;
        }

        private void SetControlledBehavioursState(bool isEnabled)
        {
            foreach (var switchable in _switchables)
            {
                if (isEnabled)
                {
                    switchable.SwitchEnable();
                }
                else
                {
                    switchable.SwitchDisable();
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            GizmosHelper.PushColor(new Color(0.8f, 0.8f, 0.4f, 0.2f));
            Gizmos.DrawSphere(transform.position, detectRadius);
            GizmosHelper.PopColor();
        }
    }
}