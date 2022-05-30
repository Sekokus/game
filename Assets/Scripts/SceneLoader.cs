﻿using System;
using System.IO;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static string InitialScene { get; set; }

    private const string ScenesPath = "Scenes";

    public event Action<string> SceneUnloaded;
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
        if (operation == null)
        {
            return;
        }
        operation.completed += _ =>
        {
            SceneUnloaded?.Invoke(name);
            completed?.Invoke();
        };
    }

    public void ReplaceLastScene(string name)
    {
        UnloadScene(CurrentScene, () => LoadScene(name));
    }

    public void ReloadLastScene() => ReplaceLastScene(CurrentScene);
}