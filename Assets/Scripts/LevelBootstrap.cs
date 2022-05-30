using DefaultNamespace;
using Enemies;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class LevelBootstrap : MonoBehaviour
{
    private LevelFactory _levelFactory;
    
    [SerializeField] private LevelType levelType;
    [SerializeField] private Marker playerMarker;
    [SerializeField] private EnemyMarker[] enemyMarkers;
    [SerializeField] private Collectable[] collectables;

    [SerializeField] private bool startWithTimer;
    private GameEvents _gameEvents;
    private LevelGoalCounter _counter;
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _gameEvents = Container.Get<GameEvents>();
        _gameEvents.PlayerDied += OnPlayerDied;

        _sceneLoader = Container.Get<SceneLoader>();
        _levelFactory = Container.Get<LevelFactory>();
        _counter = Container.Get<LevelGoalCounter>();
        _counter.SetRequiredCount(collectables?.Length ?? 0);
    }

    private void OnPlayerDied()
    {
        _sceneLoader.ReloadLastScene();
    }

    private void Start()
    {
        var levelEntry = _levelFactory.CreateLevel(levelType, playerMarker, enemyMarkers);
        levelEntry.StartLevel(startWithTimer);

        _counter.ReachedRequiredCount += () =>
        {
            _gameEvents.PostPlayerGoalCompleted();
        };
    }
}