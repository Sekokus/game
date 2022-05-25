using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sekokus.Enemies
{
    public class BulletPool
    {
        private readonly Dictionary<BulletType, Stack<Bullet>> _availableBullets =
            new Dictionary<BulletType, Stack<Bullet>>();

        private readonly BulletFactory _bulletFactory;
        private readonly int _preCreateSize;

        public BulletPool(BulletFactory bulletFactory, int preCreateSize = 16)
        {
            _bulletFactory = bulletFactory;
            _preCreateSize = preCreateSize;
            
            InitializeDictionary();
        }
        private void InitializeDictionary()
        {
            foreach (var value in Enum.GetValues(typeof(BulletType)))
            {
                _availableBullets[(BulletType)value] = new Stack<Bullet>();
            }
        }

        public void Add(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _availableBullets[bullet.BulletType].Push(bullet);
        }

        public Bullet Take(BulletType bulletType)
        {
            var bulletsOfType = _availableBullets[bulletType];
            if (bulletsOfType.Count == 0)
            {
                CreateBulletsOfType(bulletType, _preCreateSize);
            }

            var bullet = bulletsOfType.Pop();
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        private void CreateBulletsOfType(BulletType bulletType, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var bullet = _bulletFactory.CreateBullet(bulletType);
                bullet.gameObject.hideFlags = HideFlags.HideAndDontSave;
                Add(bullet);
            }
        }
    }
}