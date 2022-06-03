using UnityEngine;

namespace DefaultNamespace
{
    public class TransformRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Vector3 axis = new Vector3(0, 0, 1);

        private void Update()
        {
            transform.Rotate(axis, rotationSpeed * Time.deltaTime);
        }
    }
}