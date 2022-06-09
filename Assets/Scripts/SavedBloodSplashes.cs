using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Other/Blood Splashes", fileName = "SavedBloodSplashes", order = 0)]
    public class SavedBloodSplashes : ScriptableObject
    {
        [Serializable]
        public struct BloodSplashInfo
        {
            public int id;
            public Vector2 position;
            public Quaternion rotation;
            public int atlasIndex;
        }

        public string lastScene;

        [SerializeField] private List<BloodSplashInfo> bloodSplashes = new List<BloodSplashInfo>();

        public void AddInfo(BloodSplashInfo info)
        {
            bloodSplashes.Add(info);
        }

        public BloodSplashInfo PopLast()
        {
            var last = bloodSplashes.First();
            bloodSplashes.RemoveAt(0);
            return last;
        }

        public void Clear()
        {
            bloodSplashes.Clear();
        }

        public int Count => bloodSplashes.Count;
        public IEnumerable<BloodSplashInfo> BloodSplashes => bloodSplashes;
    }
}