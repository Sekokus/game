using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class Collectable : MonoBehaviour
    {
        private LevelGoalCounter _levelGoalCounter;
        private GameEvents _gameEvents;
        [SerializeField] private SpriteOutline spriteOutline;

        private void Awake()
        {
            _levelGoalCounter = Container.Get<LevelGoalCounter>();
            _gameEvents = Container.Get<GameEvents>();

            _gameEvents.PlayerTryCollect += OnTryCollect;
            SetCollectable(false);
        }

        private bool _isCollectable;

        private void OnTryCollect()
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

        private void OnDestroy()
        {
            _gameEvents.PlayerTryCollect -= OnTryCollect;
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
            _isCollectable = isCollectable;
            spriteOutline.SetOutline(_isCollectable);
        }
    }
}