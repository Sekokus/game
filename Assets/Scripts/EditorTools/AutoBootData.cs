#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.EditorTools
{
    [CreateAssetMenu(menuName = "Other/AutoBootData", fileName = "AutoBootData", order = 0)]
    public class AutoBootData : ScriptableObject
    {
        public List<string> closedScenes;
        public string lastActiveScene;
        public int loadAfterBootScene = -1;

        private static AutoBootData _instance;

        public static AutoBootData GetInstance()
        {
            if (_instance)
            {
                return _instance;
            }

            return _instance = AssetDatabase.LoadAssetAtPath<AutoBootData>("Assets/SaveData/AutoBootData.asset");
        }
    }
}
#endif