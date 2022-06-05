using Player;
using UnityEngine;

namespace Enemies
{
    public class RotateToPlayer : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;

        private void RotateInDirection(Vector2 direction)
        {
            var z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var newRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, z),
                rotationSpeed * Time.deltaTime);
            transform.rotation = newRotation;
        }

        private Transform _target;
        
        private void Awake()
        {
            Container.Get<PlayerFactory>().WhenPlayerAvailable(player =>
            {
                _target = player.Transform;
            });
        }

        private void Update()
        {
            if (!_target)
            {
                return;
            }

            var direction = ((Vector2)(_target.position - transform.position)).normalized;
            RotateInDirection(direction);
        }
    }
}