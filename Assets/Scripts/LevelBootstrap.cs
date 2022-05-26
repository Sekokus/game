using System;
using Player;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LevelBootstrap : MonoBehaviour
{
    private LevelFactory _levelFactory;
    private PlayerMarker _playerMarker;
        
    private void Start()
    {
        FindMarkers();
            
        _levelFactory = Container.Get<LevelFactory>();
        var levelEntry = _levelFactory.CreateLevel(_playerMarker);
        levelEntry.StartLevel();
    }

    private void FindMarkers()
    {
        _playerMarker = FindObjectOfType<PlayerMarker>();
    }
}