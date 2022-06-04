using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Enemies
{
    public class BulletShooter : MonoBehaviour
    {
        [SerializeField] private BulletType bulletType = BulletType.Default;
        [FormerlySerializedAs("bulletSpeed")] [SerializeField, Min(0)] private float defaultBulletSpeed = 1;

        private BulletPool _bulletPool;
        private TimerRunner _timerRunner;

        private void Awake()
        {
            _bulletPool = Container.Get<BulletPool>();
        }

        public void Shoot(Vector2 direction) => Shoot(direction, defaultBulletSpeed);
        
        public void Shoot(Vector2 direction, float speed)
        {
            var bullet = _bulletPool.Take(bulletType);
            bullet.transform.position = transform.position;
            bullet.Shoot(direction, speed);
        }
    }
}