using Player;
using UnityEngine;

namespace Enemies
{
    public class HomingMassShooter : MonoBehaviour, ISwitchable
    {
        [SerializeField] private GameObject homingMassPrefab;
        [SerializeField] private float shootInterval;
        [SerializeField] private float firstShotDelay;

        private bool _isShooting;
        private float _passedTime;
        private float _currentInterval;
        private Transform _target;

        private void Awake()
        {
            Container.Get<PlayerFactory>().WhenPlayerAvailable(player => { _target = player.Transform; });
            
            SwitchDisable();
        }

        private void Update()
        {
            if (!_isShooting)
            {
                return;
            }

            _passedTime += Time.deltaTime;
            if (_passedTime < _currentInterval)
            {
                return;
            }

            Shoot();
            _passedTime -= _currentInterval;
            _currentInterval = shootInterval;
        }

        private void Shoot()
        {
            var directionToPlayer = ((Vector2)(_target.position - transform.position)).normalized;
            var mass = Instantiate(homingMassPrefab, transform.position, Quaternion.identity);
            mass.transform.right = directionToPlayer;
        }

        public void SwitchEnable()
        {
            _isShooting = true;
        }

        public void SwitchDisable()
        {
            _passedTime = 0;
            _currentInterval = firstShotDelay;
            _isShooting = false;
        }
    }
}