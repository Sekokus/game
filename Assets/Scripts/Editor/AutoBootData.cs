#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Editor
{
    [CreateAssetMenu(menuName = "Other/AutoBootData", fileName = "AutoBootData", order = 0)]
    public class AutoBootData : ScriptableObject
    {
        public List<string> closedScenes;
        public string lastActiveScene;

        private static AutoBootData _instance;

        public static AutoBootData GetInstance()
        {
            if (_instance)
            {
                return _instance;
            }

            return _instance = AssetDatabase.LoadAssetAtPath<AutoBootData>("Assets/LocalSaveData/AutoBootData.asset");
        }
    }
}
#endif