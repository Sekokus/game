using UnityEngine;

namespace UI
{
    public class InGameInventoryMenu : AbstractMenu
    {
        [SerializeField] private GameObject playerBars;
        [SerializeField] private GameObject menu;

        protected override void OnShow()
        {   
            playerBars.SetActive(false);
            menu.SetActive(true);
        }

        protected override void OnClose()
        {
            playerBars.SetActive(true);
            menu.SetActive(false);
        }

        public override void OnMenuKeyAction(MenuKeyAction action)
        {
            if (action == MenuKeyAction.Inventory && !IsShown)
            {
                Show();
            }
            else if (IsShown)
            {
                Close();
            }
        }
    }
}