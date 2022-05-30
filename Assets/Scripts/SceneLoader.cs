using System;
using System.IO;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private const string ScenesPath = "Scenes";

    public event Action<string> SceneLoaded;
    public string CurrentScene { get; private set; }

    public static void SetActive(string name)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }

    public void LoadScene(string name)
    {
        CurrentScene = name;
        var scene = SceneManager.GetSceneByName(name);
        if (scene.isLoaded)
        {
            return;
        }
        var operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        operation.completed += _ =>
        {
            SetActive(name);
            SceneLoaded?.Invoke(name);
        };
    }

    private string GetScenePath(string name)
    {
        return Path.Combine(ScenesPath, name);
    }

    public void UnloadScene(string name, Action completed)
    {
        var operation = SceneManager.UnloadSceneAsync(name);
        operation.completed += _ => completed();
    }

    public void ReplaceLastScene(string name)
    {
        UnloadScene(CurrentScene, () => LoadScene(name));
    }
}