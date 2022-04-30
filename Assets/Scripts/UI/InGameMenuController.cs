using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class InGameMenuController : MonoBehaviour, IMenuContext
    {
        [SerializeField] private AbstractMenu rootPauseMenu;
        [SerializeField] private AbstractMenu rootInventoryMenu;

        private AbstractMenu _activeMenu;

        private void OnEnable()
        {
            var actions = PlayerBindingsProxy.Instance.InputBindings;
            actions.UI.Enable();
            actions.UI.Menu.performed += OnMenu;
            actions.UI.Inventory.performed += OnInventory;

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
            actions.UI.Inventory.performed -= OnInventory;
        }

        private void OnMenu(InputAction.CallbackContext context)
        {
            if (_activeMenu == null)
            {
                SetActiveMenu(rootPauseMenu);
            }

            _activeMenu.OnMenuKeyAction(MenuKeyAction.Return);
        }

        private void OnInventory(InputAction.CallbackContext context)
        {
            if (_activeMenu == null)
            {
                SetActiveMenu(rootInventoryMenu);
            }

            _activeMenu.OnMenuKeyAction(MenuKeyAction.Inventory);
        }

        public void SetActiveMenu(AbstractMenu menu)
        {
            if (_activeMenu == null)
            {
                Time.timeScale = 0f;
            }
            else if (menu == null)
            {
                Time.timeScale = 1f;
            }

            _activeMenu = menu;
        }
    }
}