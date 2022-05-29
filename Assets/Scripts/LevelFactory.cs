using Player;
using UnityEngine;

public class LevelFactory
{
    private const string LevelEntry = "LevelEntry";
    
    private readonly PlayerFactory _playerFactory;
    private readonly LevelUIFactory _levelUIFactory;
    
    private static LevelEntry _levelEntryPrefab;

    public LevelFactory(PlayerFactory playerFactory, LevelUIFactory levelUIFactory)
    {
        _playerFactory = playerFactory;
        _levelUIFactory = levelUIFactory;
    
        LoadResources();
    }

    private void LoadResources()
    {
        _levelEntryPrefab ??= Resources.Load<LevelEntry>(LevelEntry);
    }

    public LevelEntry CreateLevel(PlayerMarker playerMarker)
    {
        _levelUIFactory.CreateUI();
        var entry = Object.Instantiate(_levelEntryPrefab);
        
        _playerFactory.CreatePlayer(playerMarker.Location);
        return entry;
    }
}