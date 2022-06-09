using UnityEngine;

namespace DefaultNamespace
{
    public class DoorProgressBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] targets;

        [SerializeField] private Color lockedColor = Color.grey;
        [SerializeField] private Color notCompletedColor = Color.red;
        [SerializeField] private Color completedColor = Color.yellow;
        [SerializeField] private Color fullyCompletedColor = Color.green;

        public void FillFromLevelGroup(LevelGroup levelGroup)
        {
            var levelDatas = levelGroup.LevelDatas;
            var levelCount = levelDatas.Count;
            var isLocked = !levelGroup.IsUnlocked();

            for (var i = 0; i < targets.Length; i++)
            {
                if (i >= levelCount)
                {
                    targets[i].color = lockedColor;
                    continue;
                }

                var color = isLocked ? lockedColor : GetLevelColor(levelDatas[i]);
                targets[i].color = color;
            }
        }

        private Color GetLevelColor(LevelData levelData)
        {
            if (!levelData.IsCompleted)
            {
                return notCompletedColor;
            }

            return levelData.IsFullyCompleted ? fullyCompletedColor : completedColor;
        }
    }
}