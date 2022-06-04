using System;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelFactory
{
    private const string CollectAllLevelPath = "CollectAllLevelEntry";
    private const string HubLevelPath = "HubLevelEntry";

    private readonly PlayerFactory _playerFactory;
    private readonly EnemyFactory _enemyFactory;
    private readonly LevelUIFactory _levelUIFactory;

    private static LevelEntry _collectAllLevelPrefab;
    private static LevelEntry _hunEntryPrefab;

    public LevelFactory(PlayerFactory playerFactory, EnemyFactory enemyFactory, LevelUIFactory levelUIFactory)
    {
        _playerFactory = playerFactory;
        _enemyFactory = enemyFactory;
        _levelUIFactory = levelUIFactory;
    }

    public LevelEntry CreateLevel(LevelType levelType, Marker playerMarker, IEnumerable<EnemyMarker> enemyMarkers)
    {
        var entry = CreateLevelEntry(levelType);

        CreateUi(levelType);
        CreatePlayer(playerMarker);
        CreateEnemies(enemyMarkers);

        return entry;
    }

    private void CreateUi(LevelType levelType)
    {
        _levelUIFactory.CreateUi(levelType);
    }

    private static LevelEntry CreateLevelEntry(LevelType levelType)
    {
        var prefab = GetLevelPrefab(levelType);
        return Object.Instantiate(prefab);
    }

    private void CreatePlayer(Marker playerMarker)
    {
        _playerFactory.CreatePlayerFromMarker(playerMarker);
    }

    private void CreateEnemies(IEnumerable<EnemyMarker> enemyMarkers)
    {
        foreach (var enemyMarker in enemyMarkers)
        {
            _enemyFactory.CreateEnemyFromMarker(enemyMarker);
        }
    }

    private static LevelEntry GetLevelPrefab(LevelType levelType)
    {
        switch (levelType)
        {
            case LevelType.CollectAll:
                if (_collectAllLevelPrefab == null)
                {
                    _collectAllLevelPrefab = Resources.Load<LevelEntry>(CollectAllLevelPath);
                }

                return _collectAllLevelPrefab;
            case LevelType.Hub:
                if (_hunEntryPrefab == null)
                {
                    _hunEntryPrefab = Resources.Load<LevelEntry>(HubLevelPath);
                }

                return _hunEntryPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(levelType), levelType, null);
        }
    }
}