using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Enemies
{
    public class HomingMassShooter : MonoBehaviour, ISwitchable
    {
        [SerializeField] private Light2D headLight;
        [SerializeField] private float enabledIntensity;
        [SerializeField] private float disabledIntensity;
        
        [SerializeField] private GameObject homingMassPrefab;
        [SerializeField] private float shootInterval;
        [SerializeField] private float firstShotDelay;

        private bool _isShooting;
        private float _passedTime;
        private float _currentInterval;
        private float _currentHeadRotationSpeed;

        private void Awake()
        {
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
            var mass = Instantiate(homingMassPrefab, transform.position, Quaternion.identity);
            mass.transform.right = transform.right;
        }

        public void SwitchEnable()
        {
            _isShooting = true;
            headLight.intensity = enabledIntensity;
        }

        public void SwitchDisable()
        {
            _passedTime = 0;
            _currentInterval = firstShotDelay;
            _isShooting = false;
            headLight.intensity = disabledIntensity;
        }
    }
}