using UnityEngine;

namespace DefaultNamespace
{
    public class ChangeLevelOnReachingGoal : MonoBehaviour
    {
        [SerializeField] private string nextLevel;
        private SceneLoader _sceneLoader;

        private void Awake()
        {
            Time.timeScale = 1;
            _sceneLoader = Container.Get<SceneLoader>();
            var gameEvents = Container.Get<GameEvents>();
            gameEvents.PlayerFinished += OnFinished;
        }

        private void OnFinished()
        {
            Time.timeScale = 0;
            _sceneLoader.ReplaceLastScene(nextLevel);
        }
    }
}