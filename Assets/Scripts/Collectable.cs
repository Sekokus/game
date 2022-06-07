using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class Collectable : MonoBehaviour
    {
        private LevelGoalCounter _levelGoalCounter;
        private GameState _gameState;

        [SerializeField] private SpriteOutline spriteOutline;
        [SerializeField] private GameObject hintText;

        private void Awake()
        {
            _levelGoalCounter = Container.Get<LevelGoalCounter>();
            _gameState = Container.Get<GameState>();
            SetCollectable(false);
        }

        private void OnEnable()
        {
            if (_gameState)
            {
                _gameState.PlayerInteract += OnInteract;
            }
        }

        private void OnDisable()
        {
            if (_gameState)
            {
                _gameState.PlayerInteract -= OnInteract;
            }
        }

        private bool _isCollectable;

        private void OnInteract()
        {
            if (_isCollectable)
            {
                Collect();
            }
        }

        private void Collect()
        {
            _levelGoalCounter.IncrementCounter();
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponentInParent<PlayerCore>())
            {
                SetCollectable(true);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.GetComponentInParent<PlayerCore>())
            {
                SetCollectable(false);
            }
        }

        private void SetCollectable(bool isCollectable)
        {
            spriteOutline.enabled = isCollectable;
            hintText.SetActive(isCollectable);
            
            _isCollectable = isCollectable;
        }
    }
}