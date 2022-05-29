using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class InGamePauseMenu : AbstractMenu
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private AbstractMenu settingsMenu;
        [SerializeField] private string menuScene;

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

        public override void OnChildClosed(AbstractMenu child)
        {
            Show();
        }

        public void Quit()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(menuScene);
        }
    }
}