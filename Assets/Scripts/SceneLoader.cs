using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Sekokus
{
    public class SceneLoader
    {
        private const string ScenesPath = "Scenes";
        
        private string _lastLoadedScene;

        public event Action<string> SceneLoaded;

        public static void SetActive(string name)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        }

        public void LoadScene(string name)
        {
            _lastLoadedScene = name;

            var path = GetScenePath(name);
            var scene = SceneManager.GetSceneByName(name);
            if (scene.isLoaded)
            {
                return;
            }
            SceneManager.LoadScene(path, LoadSceneMode.Additive);
            SceneLoaded?.Invoke(name);
        }

        private string GetScenePath(string name)
        {
            return Path.Combine(ScenesPath, name);
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