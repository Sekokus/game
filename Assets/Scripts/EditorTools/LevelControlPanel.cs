using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.EditorTools
{
    public class LevelControlPanel : EditorWindow
    {
        [SerializeField] private List<LevelData> levelDatas = new List<LevelData>();
        
        [MenuItem("Window/Levels/Level Control Panel")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelControlPanel>("Level Control Panel");
            window.PopulateLevelDatas();
        }

        private void PopulateLevelDatas()
        {
            levelDatas.Clear();
            var guids = AssetDatabase.FindAssets($"t:{nameof(LevelData)}",
                new[] { "Assets/SaveData" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var levelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                levelDatas.Add(levelData);
            }

            levelDatas = levelDatas.OrderBy(ld => ld.levelName).ToList();
        }

        private static string _levelSearch = string.Empty;

        private void OnGUI()
        {
            var richTextStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };

            _levelSearch = EditorGUILayout.TextField("Search level", _levelSearch);
            
            EditorGUILayout.Space();
            foreach (var levelData in levelDatas
                         .Where(ld => ld.levelName.StartsWith(_levelSearch)))
            {
                EditorGUILayout.LabelField($"Level name: <b>{levelData.levelName}</b>", richTextStyle);
                DrawSceneInfo(levelData);

                if (GUILayout.Button($"Start at level <b>{levelData.levelName}</b>", new GUIStyle(GUI.skin.button)
                        {
                            richText = true
                        },
                        GUILayout.ExpandWidth(false)))
                {
                    var bootData = AutoBootData.GetInstance();
                    bootData.loadAfterBootScene = SceneLoader.GetBuildIndex(levelData.sceneName);
                    EditorApplication.EnterPlaymode();
                }

                EditorGUILayout.Space();
            }
        }

        private static void DrawSceneInfo(LevelData levelData)
        {
            var richTextStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };

            EditorGUILayout.BeginHorizontal();
            var sceneName = levelData.sceneName;
            EditorGUILayout.LabelField("Scene name: " + sceneName, GUILayout.ExpandWidth(false));
            var fullPath = "Assets/Scenes/" + levelData.sceneName + ".unity";
            var buildIndex = SceneUtility.GetBuildIndexByScenePath(fullPath);
            
            if (buildIndex < 0)
            {
                EditorGUILayout.LabelField("<color=red>Not in build settings</color>", richTextStyle,
                    GUILayout.ExpandWidth(false));
                if (GUILayout.Button("Add to build settings"))
                {
                    AddToBuildSettings(fullPath);
                }
            }
            else
            {
                EditorGUILayout.LabelField("<color=green>In build settings</color>", richTextStyle,
                    GUILayout.ExpandWidth(false));
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        
        public static void AddToBuildSettings(string fullScenePath)
        {
            var prevScenes = EditorBuildSettings.scenes.ToList();
            prevScenes.Add(new EditorBuildSettingsScene(fullScenePath, true));
            EditorBuildSettings.scenes = prevScenes.ToArray();
        }
    }
}