using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class MainMenuController : MonoBehaviour, IMenuContext
    {
        [SerializeField] private AbstractMenu startMenu;

        private AbstractMenu _activeMenu;

        private void OnEnable()
        {
            var actions = PlayerBindingsProxy.Instance.InputBindings;
            actions.UI.Enable();
            actions.UI.Menu.performed += OnMenu;

            var menus = GetComponentsInChildren<AbstractMenu>();
            foreach (var menu in menus)
            {
                menu.SetMenuContext(this);
            }
        }

        private void OnDisable()
        {
            var actions = PlayerBindingsProxy.Instance.InputBindings;
            actions.UI.Menu.performed -= OnMenu;
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