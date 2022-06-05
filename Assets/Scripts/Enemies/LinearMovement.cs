using UnityEngine;

namespace Enemies
{
    public class LinearMovement : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;

        private void Update()
        {
            transform.position += transform.right * (movementSpeed * Time.deltaTime);
        }
    }
}