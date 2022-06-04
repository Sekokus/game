using System;
using DefaultNamespace;
using Enemies;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class LevelBootstrap : MonoBehaviour
{
    private LevelFactory _levelFactory;

    [SerializeField] private LevelType levelType;
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
    [SerializeField] private int requiredCount = 3;

    private void Awake()
    {
        _gameEvents = Container.Get<GameEvents>();
        _gameEvents.PlayerDied += OnPlayerDied;

        _sceneLoader = Container.Get<SceneLoader>();
        _levelFactory = Container.Get<LevelFactory>();
        _counter = Container.Get<LevelGoalCounter>();

        _enemyMarkers = enemyMarkersParent.GetComponentsInChildren<EnemyMarker>();
        _collectables = collectablesParent.GetComponentsInChildren<Collectable>();

        _counter.SetCounts(requiredCount, _collectables.Length);
    }

    private void OnPlayerDied()
    {
        _sceneLoader.ReloadLastScene();
    }

    private void Start()
    {
        var levelEntry = _levelFactory.CreateLevel(levelType, playerMarker, _enemyMarkers);
        levelEntry.StartLevel(startWithTimer, startText);

        _counter.ReachedMinCount += () => { _gameEvents.PostMinGoalCompleted(); };
    }
}