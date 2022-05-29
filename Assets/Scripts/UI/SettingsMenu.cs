using UnityEngine;

namespace UI
{
    public class SettingsMenu : AbstractMenu
    {
        [SerializeField] private GameObject menu;

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
            if (action == MenuKeyAction.Return)
            {
                Close();
            }
        }
    }
}