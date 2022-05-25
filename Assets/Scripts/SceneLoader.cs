using System;
using UnityEngine.SceneManagement;

namespace Sekokus
{
    public class SceneLoader
    {
        private string _lastLoadedScene;

        public event Action<string> SceneLoaded;
        
        public void LoadScene(string name)
        {
            _lastLoadedScene = name;
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
            SceneLoaded?.Invoke(name);
        }

        public void UnloadScene(string name)
        {
            SceneManager.UnloadSceneAsync(name);
        }

        public void ReplaceLastScene(string name)
        {
            UnloadScene(_lastLoadedScene);
            LoadScene(name);
        }
    }
}