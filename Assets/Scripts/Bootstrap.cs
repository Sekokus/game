using Enemies;
using Player;
using UnityEngine;
using Utilities;

[DefaultExecutionOrder(-100)]
public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string afterLoadScene;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        var sceneLoader = Container.Get<SceneLoader>();
        sceneLoader.LoadScene(afterLoadScene);

        SceneLoader.SetActive(afterLoadScene);
    }

    private void Initialize()
    {
        BindPlayerFactory();
        BindUIFactory();
        BindBulletFactory();
        BindBulletPool();
        BindLevelFactory();

        BindPauseService();

        BindPlayerBindings();

        AddApplicationEvents();
        AddTimerRunner();
        AddCoroutineRunner();
    }

    private void AddApplicationEvents() => AddSingletonFromScene<ApplicationEvents>();

    private static void BindBulletPool()
    {
        Container.Add(() =>
        {
            var bulletFactory = Container.Get<BulletFactory>();
            return new BulletPool(bulletFactory);
        }, ServiceLifetime.PerScene);
    }

    private static void BindBulletFactory()
    {
        Container.Add<BulletFactory>(ServiceLifetime.Singleton);
    }

    private void BindPauseService()
    {
        Container.Add<PauseService>(ServiceLifetime.Singleton);
    }

    private void BindPlayerBindings()
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

    private static void BindLevelFactory()
    {
        Container.Add(() =>
        {
            var playerFactory = Container.Get<PlayerFactory>();
            var uiFactory = Container.Get<LevelUIFactory>();
            return new LevelFactory(playerFactory, uiFactory);
        }, ServiceLifetime.PerScene);
    }

    private static void BindUIFactory()
    {
        Container.Add<LevelUIFactory>(ServiceLifetime.Singleton);
    }

    private static void BindPlayerFactory()
    {
        Container.Add<PlayerFactory>(ServiceLifetime.PerScene);
    }
}