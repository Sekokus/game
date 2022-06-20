using System;
using System.IO;
using DefaultNamespace;
using UnityEngine;

public class DataSaveManager : MonoBehaviour
{
    [Serializable]
    private struct LevelData
    {
        public int bestTime;
        public int bestCount;
    }

    [SerializeField] private LevelGroup[] levelGroups;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private float autoSaveInterval;

    private static string SaveDirectory => Path.Combine(Application.persistentDataPath, "Save");
    private static string SettingsPath => Path.Combine(SaveDirectory, "settings.json");
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = Container.Get<SceneLoader>();
        _sceneLoader.SceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(int obj)
    {
        if (obj != SceneLoader.HubScene && obj != SceneLoader.MenuScene)
        {
            SaveAll();
        }
    }

    private void Start()
    {
        LoadAll();
        InvokeRepeating(nameof(SaveAll), autoSaveInterval, autoSaveInterval);
        GameSettings.Instance.PropertyValueChanged += OnSettingsUpdated;
    }

    private void OnSettingsUpdated(string obj)
    {
        SaveAll();
    }

    public void LoadAll()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        try
        {
            for (var i = 0; i < levelGroups.Length; i++)
            {
                var currentGroup = levelGroups[i];
                var currentLevelDatas = currentGroup.LevelDatas;
                for (var j = 0; j < currentLevelDatas.Count; j++)
                {
                    var currentLevelData = currentLevelDatas[j];
                    var path = Path.Combine(SaveDirectory, currentGroup.name) + "_" + currentLevelData.levelName +
                               ".json";
                    var file = File.ReadAllText(path);
                    var loadedGroup = JsonUtility.FromJson<LevelData>(file);
                    currentLevelData.bestLevelCoinCount = loadedGroup.bestCount;
                    currentLevelData.bestLevelTimeMs = loadedGroup.bestTime;
                }
            }

            var settings = JsonUtility.FromJson<GameSettings>(SettingsPath);
            gameSettings.BloodEnabled = settings.BloodEnabled;
            gameSettings.CameraShakeStrength = settings.CameraShakeStrength;
            gameSettings.ChromaticAberrationStrength = settings.ChromaticAberrationStrength;
            gameSettings.MotionBlurStrength = settings.MotionBlurStrength;
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public void SaveAll()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var jsonSettings = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(SettingsPath, jsonSettings);

        foreach (var levelGroup in levelGroups)
        {
            var levelGroupLevelDatas = levelGroup.LevelDatas;
            for (var i = 0; i < levelGroupLevelDatas.Count; i++)
            {
                var data = new LevelData()
                {
                    bestCount = levelGroupLevelDatas[i].bestLevelCoinCount,
                    bestTime = levelGroupLevelDatas[i].bestLevelTimeMs
                };

                var jsonGroup = JsonUtility.ToJson(data, true);
                File.WriteAllText(
                    Path.Combine(SaveDirectory, levelGroup.name) + "_" + levelGroupLevelDatas[i].levelName + ".json",
                    jsonGroup);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }
}