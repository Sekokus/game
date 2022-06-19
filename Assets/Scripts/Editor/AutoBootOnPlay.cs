#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Editor
{
    [InitializeOnLoad]
    public static class AutoBootOnPlay
    {
        static AutoBootOnPlay()
        {
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

        private static void LoadDefaultScene(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    StartWithOnlyBootScene();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    OpenClosedScenes();
                    var bootData = LoadBootData();
                    bootData.afterBootScene = SceneLoader.MenuScene;
                    break;
            }
        }

        public static BootData LoadBootData()
        {
            var bootDataGuid = AssetDatabase.FindAssets($"t:{nameof(BootData)}").First();
            var bootDataPath = AssetDatabase.GUIDToAssetPath(bootDataGuid);
            var bootData = AssetDatabase.LoadAssetAtPath<BootData>(bootDataPath);
            return bootData;
        }

        private static void OpenClosedScenes()
        {
            var bootData = AutoBootData.GetInstance();
            foreach (var closedScenePath in bootData.closedScenes)
            {
                EditorSceneManager.OpenScene(closedScenePath, OpenSceneMode.Additive);
            }

            var activeScene = SceneManager.GetSceneByPath(bootData.lastActiveScene);
            SceneManager.SetActiveScene(activeScene);
            var bootScene = SceneManager.GetSceneAt(0);
            if (activeScene.buildIndex != 0 && !bootData.closedScenes.Contains(bootScene.path))
            {
                EditorSceneManager.CloseScene(bootScene, true);
            }
        }

        private static void StartWithOnlyBootScene()
        {
            var bootData = AutoBootData.GetInstance();
            bootData.closedScenes.Clear();

            bootData.lastActiveScene = SceneManager.GetActiveScene().path;

            var loadedCount = SceneManager.sceneCount;
            var loadedScenes = new Scene[loadedCount];

            for (var i = 0; i < loadedCount; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            foreach (var loadedScene in loadedScenes.Where(scene => scene.buildIndex != 0))
            {
                bootData.closedScenes.Add(loadedScene.path);
            }

            EditorUtility.SetDirty(bootData);
            AssetDatabase.SaveAssetIfDirty(bootData);
            EditorSceneManager.OpenScene("Assets/Scenes/Boot.unity", OpenSceneMode.Single);
        }
    }
}
#endif