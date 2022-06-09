using System;
using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class FinishDirectionHint : MonoBehaviour
    {
        [SerializeField] private RectTransform pointer;
        [SerializeField] private int borders;

        private Transform _finish;
        private Transform _player;
        private GameState _gameState;
        private Camera _camera;

        private void Awake()
        {
            _gameState = Container.Get<GameState>();
            _gameState.PlayerMinGoalCompleted += OnMinGoalCompleted;

            _camera = Camera.main;

            Container.Get<PlayerFactory>().WhenPlayerAvailable(core => _player = core.Transform);
            _finish = FindObjectOfType<Finish>().transform;

            pointer.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _gameState.PlayerMinGoalCompleted -= OnMinGoalCompleted;
        }

        private bool _isShowingHint;

        private void OnMinGoalCompleted()
        {
            _isShowingHint = true;
            pointer.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!_isShowingHint)
            {
                return;
            }

            var playerScreenPosition = _camera.WorldToScreenPoint(_player.position);
            var finishScreenPosition = _camera.WorldToScreenPoint(_finish.position);

            var direction = (finishScreenPosition - playerScreenPosition).normalized;
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
            pointer.localEulerAngles = new Vector3(0, 0, angle);

            var isOffScreen = IsOffScreen(finishScreenPosition);
            pointer.gameObject.SetActive(isOffScreen);
            if (!isOffScreen)
            {
                return;
            }

            var clampedPosition = ClampPosition(finishScreenPosition);
            pointer.position = clampedPosition;
        }

        private Vector2 ClampPosition(Vector2 finishScreenPosition)
        {
            var clampedPosition = finishScreenPosition;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, borders, Screen.width - borders);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, borders, Screen.height - borders);
            return clampedPosition;
        }

        private bool IsOffScreen(Vector2 finishScreenPosition)
        {
            var border = new BoundsInt(0, 0, -1,
                Screen.width, Screen.height, 2);
            var isOffScreen =
                !border.Contains(new Vector3Int((int)finishScreenPosition.x, (int)finishScreenPosition.y, 0));
            return isOffScreen;
        }
    }
}