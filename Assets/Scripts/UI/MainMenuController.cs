using System;
using Sekokus;
using Sekokus.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class MainMenuController : MonoBehaviour, IMenuContext
    {
        [SerializeField] private AbstractMenu startMenu;

        private AbstractMenu _activeMenu;
        private InputBindings _bindings;

        private void Start()
        {
            _bindings = Container.Get<PlayerBindings>().GetBindings();
        }

        private void OnEnable()
        {
            _bindings.UI.Enable();
            _bindings.UI.Menu.performed += OnMenu;

            AbstractMenu[] menus = GetComponentsInChildren<AbstractMenu>();
            foreach (var menu in menus)
            {
                menu.SetMenuContext(this);
            }
        }

        private void OnDisable()
        {
            _bindings.UI.Menu.performed -= OnMenu;
        }

        private void OnMenu(InputAction.CallbackContext context)
        {
            if (_activeMenu == null)
            {
                SetActiveMenu(startMenu);
            }

            _activeMenu.OnMenuKeyAction(MenuKeyAction.Return);
        }

        public void SetActiveMenu(AbstractMenu menu)
        {
            _activeMenu = menu;
        }
    }
}