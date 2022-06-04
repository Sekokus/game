using System.Linq;
using Enemies;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class HubBootstrap : MonoBehaviour
{
    private LevelFactory _levelFactory;

    [SerializeField] private Marker playerMarker;
    [SerializeField] private string startText = "Collect enough Coins to proceed";
    private GameEvents _gameEvents;
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _levelFactory = Container.Get<LevelFactory>();
        _sceneLoader = Container.Get<SceneLoader>();
        _gameEvents = Container.Get<GameEvents>();
        _gameEvents.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        _sceneLoader.ReloadLastScene();
    }

    private void OnDestroy()
    {
        _gameEvents.PlayerDied -= OnPlayerDied;
    }
    
    private void Start()
    {
        var levelEntry = _levelFactory.CreateLevel(LevelType.Hub, playerMarker, 
            Enumerable.Empty<EnemyMarker>());
        levelEntry.StartLevel(false, startText);
    }
}