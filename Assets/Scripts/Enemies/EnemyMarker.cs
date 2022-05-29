using UnityEngine;

namespace Enemies
{
    public class EnemyMarker : Marker
    {
        [SerializeField] private EnemyType enemyType;

        public EnemyType EnemyType => enemyType;

        public override string Name => "Enemy " + EnemyType;
    }
}