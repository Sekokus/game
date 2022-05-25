using System.Collections.Generic;
using UnityEngine;

namespace Sekokus.Enemies
{
    public class BulletFactory
    {
        private const string BulletsPath = "Bullets";
        private readonly Dictionary<BulletType, Bullet> _bulletPrefabs = new Dictionary<BulletType, Bullet>();

        public BulletFactory()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            var bulletVariants = Resources.LoadAll<Bullet>(BulletsPath);
            foreach (var bulletVariant in bulletVariants)
            {
                _bulletPrefabs[bulletVariant.BulletType] = bulletVariant;
            }
        }

        public Bullet CreateBullet(BulletType bulletType)
        {
            var bulletPrefab = _bulletPrefabs[bulletType];
            return Object.Instantiate(bulletPrefab);
        }
    }
}