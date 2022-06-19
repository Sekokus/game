using System;
using System.Collections;
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
        [SerializeField] private PlayerRaycaster playerRaycaster;
        [SerializeField] private float scanInterval = 0.3f;
        [SerializeField] private float deaggroTime = 1f;

        private ISwitchable[] _switchables;

        private PlayerCore _player;

        private Coroutine _deaggroRoutine;

        private void Reset()
        {
            playerRaycaster = GetComponent<PlayerRaycaster>();
        }

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

            var directionToPlayer = ((Vector2)(_player.Transform.position - transform.position)).normalized;
            var (hit, _) = playerRaycaster.Raycast(directionToPlayer);
            SetDetectedState(hit);
        }

        private bool _detected = false;

        private void SetDetectedState(bool detected)
        {
            if (_detected == detected)
            {
                return;
            }

            if (!detected)
            {
                _deaggroRoutine = StartCoroutine(DeaggroRoutine(deaggroTime));
            }
            else
            {
                if (_deaggroRoutine != null)
                {
                    StopCoroutine(_deaggroRoutine);
                }

                SetDetectedStateImmediately(true);
            }
        }

        private void SetDetectedStateImmediately(bool detected)
        {
            SetControlledBehavioursState(detected);
            _detected = detected;
        }

        private IEnumerator DeaggroRoutine(float time)
        {
            yield return new WaitForSeconds(time);
            SetDetectedStateImmediately(false);
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
    }
}