using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class BulletFactory
    {
        private const string BulletsPath = "Bullets";
        private static readonly Dictionary<BulletType, Bullet> BulletPrefabs = new Dictionary<BulletType, Bullet>();
        private static bool _resourcesLoaded;

        public BulletFactory()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            if (_resourcesLoaded)
            {
                return;
            }
            
            var bulletVariants = Resources.LoadAll<Bullet>(BulletsPath);
            foreach (var bulletVariant in bulletVariants)
            {
                BulletPrefabs[bulletVariant.BulletType] = bulletVariant;
            }

            _resourcesLoaded = true;
        }

        public Bullet CreateBullet(BulletType bulletType)
        {
            var bulletPrefab = BulletPrefabs[bulletType];
            return Object.Instantiate(bulletPrefab);
        }
    }
}