using System;
using UnityEngine;

namespace DefaultNamespace
{
    public static class LevelLibrary
    {
        private static readonly string[] Levels =
        {
            "Test_Level_1",
            "Test_Level_2"
        };

        public static int LevelCount => Levels.Length;
        public static string GetLevelAt(int index) => Levels[index];
        public static int GetLevelIndex(string name) => Array.IndexOf(Levels, name);
    }

    public class ChangeLevelOnReachingGoal : MonoBehaviour
    {
        private LevelGoalCounter _counter;
        private SceneLoader _sceneLoader;

        private void Awake()
        {
            Time.timeScale = 1;
            _sceneLoader = Container.Get<SceneLoader>();
            _counter = Container.Get<LevelGoalCounter>();
            _counter.ReachedRequiredCount += CounterOnReachedRequiredCount;
        }

        private void CounterOnReachedRequiredCount()
        {
            var currentLevel = _sceneLoader.CurrentScene;
            var currentIndex = LevelLibrary.GetLevelIndex(currentLevel);
            if (currentIndex + 1 < LevelLibrary.LevelCount)
            {
                Time.timeScale = 0;
                _sceneLoader.ReplaceLastScene(LevelLibrary.GetLevelAt(currentIndex + 1));
            }
        }
    }
}