using UnityEngine;

namespace DefaultNamespace
{
    public class TransformTweener : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        [SerializeField] private float moveTime;
        private Vector2 _initialPosition;

        private int _moveSign = 1;
        private float _currentTime01;

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        private void Update()
        {
            UpdateCurrentMoveTime();
            UpdateMoveSingIfNeeded();
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.position = _initialPosition + offset * _currentTime01;
        }

        private void UpdateCurrentMoveTime()
        {
            _currentTime01 = Mathf.Clamp01(_currentTime01 + _moveSign * Time.deltaTime / moveTime);
        }

        private void UpdateMoveSingIfNeeded()
        {
            _moveSign = _moveSign switch
            {
                1 when Mathf.Approximately(_currentTime01, 1) => -1,
                -1 when Mathf.Approximately(_currentTime01, 0) => 1,
                _ => _moveSign
            };
        }
    }
}