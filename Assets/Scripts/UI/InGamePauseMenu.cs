using UnityEngine;

namespace UI
{
    public class InGamePauseMenu : AbstractMenu
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private AbstractMenu settingsMenu;
        private GameState _gameState;
        private SceneLoader _sceneLoader;

        private void Awake()
        {
            _gameState = Container.Get<GameState>();
            _sceneLoader = Container.Get<SceneLoader>();
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
            if (action != MenuKeyAction.Return)
            {
                return;
            }

            if (IsShown)
            {
                Close();
            }
            else
            {
                Time.timeScale = 0;
                Show();
            }
        }

        public void ShowSettings()
        {
            Close();
            settingsMenu.Show(this);
        }

        public void Restart()
        {
            Close();
            _gameState.PostPlayerDied();
        }

        public override void OnChildClosed(AbstractMenu child)
        {
            Show();
        }

        public void Quit()
        {
            Time.timeScale = 1;
            _sceneLoader.ReplaceLastScene(SceneLoader.MenuScene);
        }

        public void QuitToHub()
        {
            Time.timeScale = 1;
            _sceneLoader.ReplaceLastScene(SceneLoader.HubScene);
        }
    }
}