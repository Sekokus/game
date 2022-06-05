using DefaultNamespace;
using Player;
using UnityEngine;

public class LevelGroupDoor : MonoBehaviour
{
    [SerializeField] private LevelGroup levelGroup;
    [SerializeField] private GameObject hint;
    [SerializeField] private PlayerTriggerEvents triggerEvents;
    [SerializeField] private DoorProgressBar progressBar;

    private GameEvents _gameEvent;
    private bool _canInteract;
    private LevelGroupUi _groupUi;

    private void Awake()
    {
        _groupUi = Container.Get<LevelGroupUi>();
        
        _gameEvent = Container.Get<GameEvents>();
        _gameEvent.PlayerInteract += OnInteracted;

        triggerEvents.Entered += OnEntered;
        triggerEvents.Exited += OnExited;

        hint.SetActive(false);
        
        progressBar.FillFromLevelGroup(levelGroup);
    }

    private bool _isShowingUi;
    
    private void OnInteracted()
    {
        if (!_canInteract || _isShowingUi)
        {
            return;
        }

        _isShowingUi = true;
        hint.SetActive(false);
        _groupUi.ShowFromLevelGroup(levelGroup);
    }

    private void OnExited(PlayerCore obj)
    {
        _canInteract = false;
        hint.SetActive(false);

        if (!_isShowingUi)
        {
            return;
        }
        
        _groupUi.Hide();
        _isShowingUi = false;
    }

    private void OnEntered(PlayerCore obj)
    {
        _canInteract = true;
        hint.SetActive(true);
    }

    private void OnDestroy()
    {
        _gameEvent.PlayerInteract -= OnInteracted;
    }
}