﻿using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class InGameMenuController : MonoBehaviour, IMenuContext
    {
        [SerializeField] private AbstractMenu rootPauseMenu;
        [SerializeField] private AbstractMenu rootInventoryMenu;

        private AbstractMenu _activeMenu;
        private InputBindings _bindings;
        private PauseService _pauseService;

        private void Awake()
        {
            Construct();
        }

        private void Construct()
        {
            _pauseService = Container.Get<PauseService>();
            
            _bindings = Container.Get<PlayerBindings>().GetBindings();
            _bindings.UI.Enable();
            _bindings.UI.Menu.performed += OnMenu;

            var menus = GetComponentsInChildren<AbstractMenu>();
            foreach (var menu in menus)
            {
                menu.SetMenuContext(this);
            }
        }

        private void OnDestroy()
        {
            _bindings.UI.Menu.performed -= OnMenu;
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
            if (_activeMenu == null)
            {
                _pauseService.Pause(PauseSource.MenuOpened);
            }
            else if (menu == null)
            {
                _pauseService.Unpause(PauseSource.MenuOpened);
            }

            _activeMenu = menu;
        }

        private void OnDisable()
        {
            _pauseService.Unpause(PauseSource.MenuOpened);
        }
    }
}