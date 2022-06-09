using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class InGameMenuController : MonoBehaviour, IMenuContext
    {
        [SerializeField] private AbstractMenu rootPauseMenu;

        private AbstractMenu _activeMenu;
        private InputBindings _bindings;
        private GameState _gameState;

        private void OnEnable()
        {
            _gameState.StateChanged += OnStateChanged;

            _bindings.UI.Enable();
            _bindings.UI.Menu.performed += OnMenu;
        }

        private void OnDisable()
        {
            _gameState.StateChanged -= OnStateChanged;
            _bindings.UI.Menu.performed -= OnMenu;
            
            _gameState.SetState(GameStateType.Default);
        }

        private void OnStateChanged(GameStateType newState, GameStateType oldState)
        {
            if (newState == GameStateType.LevelSelectionOpened)
            {
                _bindings.UI.Menu.performed -= OnMenu;
            }
            else if (oldState == GameStateType.LevelSelectionOpened)
            {
                _bindings.UI.Menu.performed += OnMenu;
            }
        }

        private void Awake()
        {
            Construct();
        }

        private void Construct()
        {
            _gameState = Container.Get<GameState>();
            _bindings = Container.Get<PlayerBindings>().GetBindings();

            var menus = GetComponentsInChildren<AbstractMenu>();
            foreach (var menu in menus)
            {
                menu.SetMenuContext(this);
            }
        }

        private void OnMenu(InputAction.CallbackContext context)
        {
            if (_activeMenu == null)
            {
                SetActiveMenu(rootPauseMenu);
            }

            _activeMenu.OnMenuKeyAction(MenuKeyAction.Return);
        }

        public void SetActiveMenu(AbstractMenu menu)
        {
            if (_activeMenu != null && menu == null)
            {
                _gameState.SetState(GameStateType.Default);
            }
            else if (_activeMenu == null && menu != null)
            {
                _gameState.SetState(GameStateType.MenuOpened);
            }
            
            _activeMenu = menu;
            Time.timeScale = _activeMenu != null ? 0 : 1;
        }
    }
}