using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Enemies
{
    public class EnemyFactory
    {
        private const string EnemiesFolderPath = "Enemies";

        private static readonly Dictionary<EnemyType, string> EnemyTypePaths = new Dictionary<EnemyType, string>
        {
            { EnemyType.Turret, "Turret" }
        };

        private static readonly Dictionary<EnemyType, EnemyConductor> LoadedEnemyPrefabs =
            new Dictionary<EnemyType, EnemyConductor>();

        private static EnemyConductor LoadEnemyResource(EnemyType enemyType)
        {
            var fullPath = Path.Combine(EnemiesFolderPath, EnemyTypePaths[enemyType]);
            var prefab = Resources.Load<EnemyConductor>(fullPath);
            LoadedEnemyPrefabs[enemyType] = prefab;
            return prefab;
        }

        public EnemyConductor CreateEnemyFromMarker(EnemyMarker enemyMarker)
        {
            var prefab = GetEnemyPrefab(enemyMarker.EnemyType);
            var enemy = Object.Instantiate(prefab);

            Configure(enemy, enemyMarker);

            return enemy;
        }

        private static void Configure(EnemyConductor enemy, Marker enemyMarker)
        {
            var transform = enemy.transform;
            transform.position = enemyMarker.Location;
            transform.rotation = enemyMarker.Rotation;
        }

        private static EnemyConductor GetEnemyPrefab(EnemyType enemyType)
        {
            if (!LoadedEnemyPrefabs.TryGetValue(enemyType, out var prefab))
            {
                prefab = LoadEnemyResource(enemyType);
            }

            return prefab;
        }
    }
}