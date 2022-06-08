using System;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelGroupDoor : MonoBehaviour
{
    [SerializeField] private LevelGroup levelGroup;
    [SerializeField] private GameObject hint;
    [SerializeField] private PlayerTriggerEvents triggerEvents;
    [SerializeField] private DoorProgressBar progressBar;

    private GameState _gameState;
    private bool _canInteract;
    private LevelGroupUi _groupUi;

    private void Awake()
    {
        _groupUi = Container.Get<LevelGroupUi>();

        _gameState = Container.Get<GameState>();
        _bindings = Container.Get<PlayerBindings>().GetBindings();

        triggerEvents.Entered += OnEntered;
        triggerEvents.Exited += OnExited;

        hint.SetActive(false);

        progressBar.FillFromLevelGroup(levelGroup);
    }

    private void OnEnable()
    {
        _gameState.PlayerInteract += OnInteracted;
        _gameState.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameStateType newState, GameStateType oldState)
    {
        if (newState == GameStateType.MenuOpened)
        {
            _gameState.PlayerInteract -= OnInteracted;
        }

        if (oldState == GameStateType.MenuOpened)
        {
            _gameState.PlayerInteract += OnInteracted;
        }
    }

    private bool _isShowingUi;
    private InputBindings _bindings;

    private void OnInteracted()
    {
        if (!_canInteract || _isShowingUi)
        {
            return;
        }

        _isShowingUi = true;
        hint.SetActive(false);
        _groupUi.ShowFromLevelGroup(levelGroup);

        _bindings.UI.Menu.performed += OnMenu;

        _gameState.SetState(GameStateType.LevelSelectionOpened);
    }

    private void OnMenu(InputAction.CallbackContext obj)
    {
        HideSelection();
        hint.SetActive(true);
    }

    private void OnExited(PlayerCore obj)
    {
        _canInteract = false;
        hint.SetActive(false);
        
        if (!_isShowingUi)
        {
            return;
        }
        
        HideSelection();
    }

    private void HideSelection()
    {
        if (!_isShowingUi)
        {
            return;
        }

        _groupUi.Hide();
        _isShowingUi = false;

        _bindings.UI.Menu.performed -= OnMenu;
        _gameState.SetState(GameStateType.Default);
    }

    private void OnEntered(PlayerCore obj)
    {
        _canInteract = true;
        hint.SetActive(true);
    }

    private void OnDisable()
    {
        _gameState.PlayerInteract -= OnInteracted;
        _bindings.UI.Menu.performed -= OnMenu;
    }
}