using DefaultNamespace;
using Enemies;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class LevelBootstrap : MonoBehaviour
{
    private LevelFactory _levelFactory;

    [SerializeField] private Marker playerMarker;
    [SerializeField] private GameObject enemyMarkersParent;
    [SerializeField] private GameObject collectablesParent;
    [SerializeField] private string startText = "Collect enough Coins to proceed";
    [SerializeField] private bool startWithTimer;

    private GameEvents _gameEvents;
    private LevelGoalCounter _counter;
    private SceneLoader _sceneLoader;
    private EnemyMarker[] _enemyMarkers;
    private Collectable[] _collectables;

    [SerializeField] private LevelData levelData;

    private float _startTime;

    private void Awake()
    {
        _gameEvents = Container.Get<GameEvents>();
        _gameEvents.PlayerDied += OnPlayerDied;
        _gameEvents.PlayerFinished += OnPlayerFinished;

        _sceneLoader = Container.Get<SceneLoader>();
        _levelFactory = Container.Get<LevelFactory>();
        _counter = Container.Get<LevelGoalCounter>();

        _enemyMarkers = enemyMarkersParent.GetComponentsInChildren<EnemyMarker>();
        _collectables = collectablesParent.GetComponentsInChildren<Collectable>();

        if (levelData.maxCoinCount != _collectables.Length)
        {
            Debug.LogWarning("Level Data coin count != collectables count.");
            levelData.maxCoinCount = _collectables.Length;
        }

        _counter.SetCounts(levelData.requiredCoinCount, levelData.maxCoinCount);
    }

    private void OnPlayerDied()
    {
        _sceneLoader.ReloadLastScene();
    }

    private void OnPlayerFinished()
    {
        UpdateCurrentLevelData();

        var nextLevel = levelData.nextLevel;
        var nextLevelName = nextLevel != null ? nextLevel.sceneName : SceneLoader.HubScene;
        _sceneLoader.ReplaceLastScene(nextLevelName);
    }

    private void UpdateCurrentLevelData()
    {
        var passedTime = Mathf.CeilToInt((Time.time - _startTime) * 1000);
        var newCoinCount = _counter.CurrentCount;

        if (newCoinCount > levelData.bestLevelCoinCount)
        {
            levelData.bestLevelCoinCount = newCoinCount;
            levelData.bestLevelTimeMs = passedTime;
        }
        else if (newCoinCount == levelData.bestLevelCoinCount)
        {
            levelData.bestLevelTimeMs = Mathf.Min(levelData.bestLevelTimeMs, passedTime);
        }
    }

    private void Start()
    {
        var levelEntry = _levelFactory.CreateLevel(LevelType.CollectAll, playerMarker, _enemyMarkers);
        levelEntry.StartLevel(startWithTimer, startText);

        _counter.ReachedMinCount += () => { _gameEvents.PostMinGoalCompleted(); };

        _startTime = Time.time;
    }

    private void OnDestroy()
    {
        _gameEvents.PlayerDied -= OnPlayerDied;
        _gameEvents.PlayerFinished -= OnPlayerFinished;
    }
}