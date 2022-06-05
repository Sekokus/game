using UnityEngine;

namespace DefaultNamespace
{
    public class ChangeLevelOnReachingGoal : MonoBehaviour
    {
        [SerializeField] private string nextLevel;
        private SceneLoader _sceneLoader;
        private GameEvents _gameEvents;

        private void Awake()
        {
            Time.timeScale = 1;
            _sceneLoader = Container.Get<SceneLoader>();
            _gameEvents = Container.Get<GameEvents>();
            _gameEvents.PlayerFinished += OnFinished;
        }

        private void OnFinished()
        {
            Time.timeScale = 0;
            _sceneLoader.ReplaceLastScene(nextLevel);
        }

        private void OnDestroy()
        {
            _gameEvents.PlayerFinished -= OnFinished;
        }
    }
}