using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class Collectable : MonoBehaviour
    {
        private LevelGoalCounter _levelGoalCounter;
        private GameEvents _gameEvents;

        [SerializeField] private SpriteOutline spriteOutline;
        [SerializeField] private GameObject hintText;

        private void Awake()
        {
            _levelGoalCounter = Container.Get<LevelGoalCounter>();
            _gameEvents = Container.Get<GameEvents>();

            _gameEvents.PlayerInteract += OnInteract;
            SetCollectable(false);
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

        private void OnDestroy()
        {
            _gameEvents.PlayerInteract -= OnInteract;
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