using Enemies;
using UnityEngine;

namespace DefaultNamespace
{
    public class DebugAggroRange : MonoBehaviour
    {
        [SerializeField] private Color color = new Color(0.7f, 0.2f, 0.2f, 0.3f);
        [SerializeField] private EnemyType enemyType;
        [SerializeField, HideInInspector] private EnemyMarker marker;

        private void Reset()
        {
            marker = GetComponent<EnemyMarker>();
        }

        private void OnDrawGizmos()
        {
            if (marker != null)
            {
                enemyType = marker.EnemyType;
            }

            GizmosHelper.PushColor(color);

            var range = enemyType switch
            {
                EnemyType.BulletTurret => 5f,
                EnemyType.FireTurret => 2.4f,
                EnemyType.HomingMassTurret => 10f,
                _ => 0f
            };

            Gizmos.DrawSphere(transform.position, range);
            GizmosHelper.PopColor();
        }
    }
}