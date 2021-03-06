using DefaultNamespace;
using Enemies;
using Player;
using UnityEngine;
using Utilities;

[DefaultExecutionOrder(-100)]
public class Bootstrap : MonoBehaviour
{
    [SerializeField] private BootData bootData;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        ApplyGraphicsSettings();
        
        var sceneLoader = Container.Get<SceneLoader>();
        sceneLoader.LoadScene(bootData.afterBootScene);
    }

    private void ApplyGraphicsSettings()
    {
        QualitySettings.vSyncCount = 1;
    }

    private void Initialize()
    {
        BindPlayerFactory();
        BindUIFactory();
        BindBulletFactory();
        BindBulletPool();
        BindLevelFactory();
        BindEnemyFactory();
        BindCollectableAccumulator();

        BindPauseService();

        BindPlayerBindings();

        AddGameEvents();
        AddTimerRunner();
        AddCoroutineRunner();
        AddLevelGroupUi();
        AddCameraContainer();
        AddBloodSplashFactory();
    }

    private void AddBloodSplashFactory()
    {
        AddDelayedSceneSingleton<BloodSplashFactory>();
    }

    private void AddCameraContainer()
    {
        AddDelayedSceneSingleton<CameraContainer>();
    }

    private void AddLevelGroupUi()
    {
        AddDelayedSceneSingleton<LevelGroupUi>();
    }

    private static void BindCollectableAccumulator()
    {
        Container.Add<LevelGoalCounter>(ServiceLifetime.PerScene);
    }

    private static void BindEnemyFactory()
    {
        Container.Add<EnemyFactory>(ServiceLifetime.PerScene);
    }

    private void AddGameEvents() => AddSingletonFromScene<GameState>();

    private void AddDelayedSceneSingleton<TMonoBehaviour>() where TMonoBehaviour : MonoBehaviour
    {
        Container.Add(FindObjectOfType<TMonoBehaviour>, ServiceLifetime.PerScene);
    }

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
        Container.Add<BulletFactory>(ServiceLifetime.PerScene);
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
            var enemyFactory = Container.Get<EnemyFactory>();
            return new LevelFactory(playerFactory, enemyFactory, uiFactory);
        }, ServiceLifetime.PerScene);
    }

    private static void BindUIFactory()
    {
        Container.Add<LevelUIFactory>(ServiceLifetime.PerScene);
    }

    private static void BindPlayerFactory()
    {
        Container.Add<PlayerFactory>(ServiceLifetime.PerScene);
    }
}