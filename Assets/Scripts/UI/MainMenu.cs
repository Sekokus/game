﻿using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : AbstractMenu
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private AbstractMenu settingsMenu;
        [SerializeField] private string gameScene;

        private void Awake()
        {
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
            Time.timeScale = 1;
            SceneManager.LoadScene(gameScene);
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