using System;
using System.Linq;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelGroupDoor : MonoBehaviour
{
    [SerializeField] private LevelGroup levelGroup;
    [SerializeField] private GameObject hintInteract;
    [SerializeField] private GameObject hintLocked;
    [SerializeField] private GameObject lockedText;
    [SerializeField] private float lockedTextDisappearTime = 2;
    [SerializeField] private PlayerTriggerEvents triggerEvents;
    [SerializeField] private DoorProgressBar progressBar;
    [SerializeField] private SpriteRenderer[] changeColorIfAllCompleted;
    [SerializeField] private Color completedPipeColor = Color.green;

    private GameState _gameState;
    private bool _canInteract;
    private GameObject _hint;
    private LevelGroupUi _groupUi;

    private void Awake()
    {
        _groupUi = Container.Get<LevelGroupUi>();

        _gameState = Container.Get<GameState>();
        _bindings = Container.Get<PlayerBindings>().GetBindings();

        triggerEvents.Entered += OnEntered;
        triggerEvents.Exited += OnExited;

        hintInteract.SetActive(false);
        hintLocked.SetActive(false);
        lockedText.SetActive(false);

        _hint = levelGroup.IsUnlocked() ? hintInteract : hintLocked;

        progressBar.FillFromLevelGroup(levelGroup);

        SetPipesColor();
    }

    private void SetPipesColor()
    {
        if (changeColorIfAllCompleted == null || levelGroup.LevelDatas.Any(ld => !ld.IsCompleted))
        {
            return;
        }

        foreach (var spriteRenderer in changeColorIfAllCompleted)
        {
            spriteRenderer.color = completedPipeColor;
        }
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

        if (!levelGroup.IsUnlocked())
        {
            InformGroupLocked();
            return;
        }

        _isShowingUi = true;
        _hint.SetActive(false);
        _groupUi.ShowFromLevelGroup(levelGroup);

        _bindings.UI.Menu.performed += OnMenu;

        _gameState.SetState(GameStateType.LevelSelectionOpened);
    }

    private void InformGroupLocked()
    {
        lockedText.SetActive(true);
        StopAllCoroutines();
        Do.After(() => { lockedText.SetActive(false); }, lockedTextDisappearTime).Start(this);
    }

    private void OnMenu(InputAction.CallbackContext obj)
    {
        HideSelection();
        hintInteract.SetActive(true);
    }

    private void OnExited(PlayerCore obj)
    {
        _canInteract = false;
        _hint.SetActive(false);

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
        _hint.SetActive(true);
    }

    private void OnDisable()
    {
        _gameState.PlayerInteract -= OnInteracted;
        _bindings.UI.Menu.performed -= OnMenu;
    }
}