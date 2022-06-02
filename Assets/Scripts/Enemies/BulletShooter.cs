using System;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class BulletShooter : MonoBehaviour, ISwitchable
    {
        [SerializeField] private BulletType bulletType = BulletType.Default;
        [SerializeField, Min(0)] private float bulletSpeed = 1;
        [SerializeField, Min(0)] private float shootInterval = 1;

        private BulletPool _bulletPool;
        private TimerRunner _timerRunner;
        private readonly Timer _shootTimer = new Timer();

        private void Awake()
        {
            Construct();
            StartTimer();
        }

        private void StartTimer()
        {
            _shootTimer.Timeout += Shot;
            _shootTimer.Start(shootInterval, true);

            var offset = Random.Range(0, shootInterval);
            _shootTimer.Tick(offset);
        }

        private void Shot()
        {
            var direction = GetShotDirection();
            var bullet = _bulletPool.Take(bulletType);
            bullet.transform.position = transform.position;

            bullet.Shot(direction, bulletSpeed);
        }

        private Vector2 GetShotDirection()
        {
            return transform.right;
        }

        private void Construct()
        {
            _bulletPool = Container.Get<BulletPool>();
        }

        private void OnDrawGizmos()
        {
            var shotPosition = transform.position;

            GizmosHelper.PushColor(Color.yellow);

            Gizmos.DrawSphere(shotPosition, 0.3f);
            Gizmos.DrawLine(shotPosition, shotPosition + (Vector3)GetShotDirection());

            GizmosHelper.PopColor();
        }

        public void SwitchEnable()
        {
            _shootTimer.IsPaused = false;
        }

        public void SwitchDisable()
        {
            _shootTimer.IsPaused = true;
        }

        private void Update()
        {
            _shootTimer.Tick(Time.deltaTime);
        }
    }
}