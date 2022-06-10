using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Editor
{
    public class LevelCreatorWindow : EditorWindow
    {
        [SerializeField] private List<LevelGroup> levelGroups = new List<LevelGroup>();

        [MenuItem("Window/Levels/Level Creator")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelCreatorWindow>("Level Creator");
            window.PopulateLevelGroups();
        }

        private string _levelGroup = string.Empty;
        private string _levelName = string.Empty;

        [SerializeField] private Scene lastCreatedScene;

        private void PopulateLevelGroups()
        {
            levelGroups.Clear();
            var guids = AssetDatabase.FindAssets($"t:{nameof(LevelGroup)}",
                new[] { "Assets/SaveData" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var levelGroup = AssetDatabase.LoadAssetAtPath<LevelGroup>(path);
                levelGroups.Add(levelGroup);
            }

            levelGroups = levelGroups.OrderBy(lg => lg.name).ToList();
        }

        private void OnGUI()
        {
            _levelGroup = EditorGUILayout.TextField("Group:", _levelGroup, GUILayout.ExpandWidth(false));
            _levelName = EditorGUILayout.TextField("Name:", _levelName, GUILayout.ExpandWidth(false));

            var existingLevelGroup = levelGroups.FirstOrDefault(lg => lg.name == _levelGroup);
            var existingLevelData = existingLevelGroup
                ? existingLevelGroup.LevelDatas?.FirstOrDefault(data => data.levelName == _levelName)
                : null;

            GUI.enabled = existingLevelGroup == null && _levelGroup.Length >= 3;

            if (GUILayout.Button("Create Group", GUILayout.ExpandWidth(false)))
            {
                CreateLevelGroup(_levelGroup);
            }

            GUI.enabled = existingLevelGroup != null && existingLevelData == null && _levelName.Length >= 3;

            if (GUILayout.Button("Create Level", GUILayout.ExpandWidth(false)))
            {
                CreateLevel(existingLevelGroup, _levelName);
            }

            GUI.enabled = true;

            if (!lastCreatedScene.IsValid())
            {
                return;
            }

            var buildIndex = SceneLoader.GetBuildIndex(lastCreatedScene.name);
            if (buildIndex >= 0)
            {
                return;
            }

            if (GUILayout.Button($"Add {lastCreatedScene.name} to build settings?"))
            {
                LevelControlPanel.AddToBuildSettings(lastCreatedScene.path);
                lastCreatedScene = new Scene();
            }
        }

        private void CreateLevel(LevelGroup levelGroup, string levelName)
        {
            var levelData = CreateInstance<LevelData>();
            levelData.name = levelName;
            levelData.levelName = levelName;
            var sceneName = $"{levelGroup.name}/Scene_{levelName}";
            levelData.sceneName = sceneName;
            levelGroup.AddLevelData(levelData);

            AssetDatabase.CreateAsset(levelData, $"Assets/SaveData/{levelGroup.name}/{levelName}.asset");

            var template =
                AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>("Assets/Scenes/Level_Template.scenetemplate");
            var result = SceneTemplateService.Instantiate(template, false);

            lastCreatedScene = result.scene;
            AssetDatabase.CreateFolder("Assets/Scenes", levelGroup.name);
            EditorSceneManager.SaveScene(lastCreatedScene, $"Assets/Scenes/{sceneName}.unity");

            var bootstrap = FindObjectOfType<LevelBootstrap>();
            bootstrap.levelData = levelData;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = levelData;
        }

        private void CreateLevelGroup(string levelGroupName)
        {
            var levelGroup = CreateInstance<LevelGroup>();
            levelGroup.name = levelGroupName;

            AssetDatabase.CreateAsset(levelGroup, $"Assets/SaveData/{levelGroupName}.asset");
            AssetDatabase.CreateFolder("Assets/SaveData", levelGroupName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = levelGroup;

            levelGroups.Add(levelGroup);
        }
    }
}