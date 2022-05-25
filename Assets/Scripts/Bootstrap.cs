using Sekokus.Player;
using Sekokus.Utilities;
using UnityEngine;

namespace Sekokus
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private bool loadSceneOnStart;
        [SerializeField] private string afterLoadScene;

        public void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            if (!loadSceneOnStart)
            {
                return;
            }

            var sceneLoader = Container.Get<SceneLoader>();
            sceneLoader.LoadScene(afterLoadScene);
        }

        private void Initialize()
        {
            CreateFactories();
            CreatePauseService();

            AddTimerRunner();
            AddCoroutineRunner();

            CreatePlayerBindings();
        }

        private void CreatePauseService()
        {
            Container.Add<PauseService>(ServiceLifetime.Singleton);
        }

        private void CreatePlayerBindings()
        {
            Container.Add<PlayerBindings>(ServiceLifetime.Singleton);
        }

        private void AddCoroutineRunner() => AddSingletonFromScene<CoroutineRunner>();

        private void AddTimerRunner() => AddSingletonFromScene<TimerRunner>();

        private void AddSingletonFromScene<TMonoBehaviour>() where TMonoBehaviour : MonoBehaviour
        {
            var monoBehaviour = FindObjectOfType<TMonoBehaviour>();
            if (!monoBehaviour)
            {
                Debug.LogError($"Cannot find {nameof(TMonoBehaviour)}");
                return;
            }

            Container.AddSingletonInstance(monoBehaviour);
        }

        private void CreateFactories()
        {
            Container.Add<PlayerFactory>(ServiceLifetime.PerScene);
            Container.Add<UIFactory>(ServiceLifetime.PerScene);

            Container.Add(() =>
            {
                var playerFactory = Container.Get<PlayerFactory>();
                var uiFactory = Container.Get<UIFactory>();
                return new LevelFactory(playerFactory, uiFactory);
            }, ServiceLifetime.PerScene);
        }
    }
}