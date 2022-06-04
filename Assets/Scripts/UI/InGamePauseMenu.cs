using UnityEngine;

namespace UI
{
    public class InGamePauseMenu : AbstractMenu
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private AbstractMenu settingsMenu;
        private GameEvents _gameEvents;
        private SceneLoader _sceneLoader;

        private void Awake()
        {
            _gameEvents = Container.Get<GameEvents>();
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
            _gameEvents.PostPlayerDied();
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