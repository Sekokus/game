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
    
    private void Awake()
    {
        _levelFactory = Container.Get<LevelFactory>();
        var counter = Container.Get<LevelGoalCounter>();
        counter.SetRequiredCount(collectables?.Length ?? 0);
    }

    private void Start()
    {
        var levelEntry = _levelFactory.CreateLevel(levelType, playerMarker, enemyMarkers);
        levelEntry.StartLevel();
    }
}