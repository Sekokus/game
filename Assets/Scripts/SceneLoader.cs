using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public const int HubScene = 1;
    public const int MenuScene = 2;

    public event Action<int> SceneUnloaded;
    public int CurrentScene { get; private set; }

    public static void SetActive(int buildIndex)
    {
        var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
        SceneManager.SetActiveScene(scene);
    }

    public static int GetBuildIndex(string name)
    {
        var scenePath = "Assets/Scenes/" + name + ".unity";
        return SceneUtility.GetBuildIndexByScenePath(scenePath);
    }

    public void LoadScene(int buildIndex)
    {
        var scene = GetScene(buildIndex);
        CurrentScene = buildIndex;
        
        var operation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        operation.completed += _ =>
        {
            SetActive(buildIndex);
        };
    }

    private static Scene GetScene(int buildIndex)
    {
        return SceneManager.GetSceneByBuildIndex(buildIndex);
    }

    public void UnloadScene(int buildIndex, Action completed)
    {
        var operation = SceneManager.UnloadSceneAsync(buildIndex);
        if (operation == null)
        {
            return;
        }

        operation.completed += _ =>
        {
            SceneUnloaded?.Invoke(buildIndex);
            completed?.Invoke();
        };
    }

    public void ReplaceLastScene(int buildIndex)
    {
        UnloadScene(CurrentScene, () => LoadScene(buildIndex));
    }

    public void ReloadLastScene() => ReplaceLastScene(CurrentScene);
}