using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class ConstrainToCursor : MonoBehaviour
    {
        [SerializeField] private bool constrainRotation;
        [SerializeField] private bool constrainPosition;


        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            var cursorScreenPosition = Mouse.current.position.ReadValue();
            var cursorWorldPosition = _camera.ScreenToWorldPoint(cursorScreenPosition);

            if (constrainRotation)
            {
                ConstrainRotation(cursorWorldPosition);
            }

            if (constrainPosition)
            {
                ConstrainPosition(cursorWorldPosition);
            }
        }

        private void ConstrainPosition(Vector3 cursorWorldPosition)
        {
            transform.position = cursorWorldPosition;
        }

        private void ConstrainRotation(Vector3 cursorWorldPosition)
        {
            var direction = cursorWorldPosition - transform.position;
            var euler = transform.localEulerAngles;
            euler.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = euler;
        }
    }
}