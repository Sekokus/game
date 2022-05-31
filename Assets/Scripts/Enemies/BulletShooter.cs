using UnityEngine;
using Utilities;

namespace Enemies
{
    public class BulletShooter : MonoBehaviour
    {
        [SerializeField] private BulletType bulletType = BulletType.Default;
        [SerializeField, Min(0)] private float bulletSpeed = 1;
        [SerializeField, Min(0)] private float shootInterval = 1;

        private BulletPool _bulletPool;
        private TimerRunner _timerRunner;
        private Timer _shootTimer;

        private void Awake()
        {
            Construct();

            StartTimer();
        }

        private void StartTimer()
        {
            _shootTimer = _timerRunner.CreateTimer(Shot);
            _shootTimer.Start(shootInterval, true);
            
            var offset = Random.Range(0, shootInterval);
            _shootTimer.Tick(offset);
        }

        private void OnDestroy()
        {
            _timerRunner.RemoveTimer(_shootTimer);
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
            _timerRunner = Container.Get<TimerRunner>();
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
    }
}