using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : AbstractMenu
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private AbstractMenu settingsMenu;
        [SerializeField] private Button continueButton;

        private SceneLoader _sceneLoader;

        private void Awake()
        {
            _sceneLoader = Container.Get<SceneLoader>();
            
            IsShown = true;
        }

        protected override void OnShow()
        {
            menu.SetActive(true);
        }

        protected override void OnClose()
        {
            menu.SetActive(false);
        }

        public override void OnMenuKeyAction(MenuKeyAction action)
        {
        }

        public override void OnChildClosed(AbstractMenu child)
        {
            Show();
        }

        [UsedImplicitly]
        public void LoadGame()
        {
            _sceneLoader.ReplaceLastScene(SceneLoader.HubScene);
        }

        [UsedImplicitly]
        public void LoadNewGame()
        {
            _sceneLoader.ReplaceLastScene(SceneLoader.HubScene);
        }

        [UsedImplicitly]
        public void ShowSettings()
        {
            Close();
            settingsMenu.Show(this);
        }

        [UsedImplicitly]
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}