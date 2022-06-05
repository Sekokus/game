using UnityEngine;

namespace DefaultNamespace
{
    public class DoorProgressBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer levelStatusPrefab;

        [SerializeField] private Color notCompletedColor = Color.red;
        [SerializeField] private Color completedColor = Color.yellow;
        [SerializeField] private Color fullyCompletedColor = Color.green;

        [SerializeField] private Transform placeTarget;
        [SerializeField] private Transform endPlaceTarget;

        public void FillFromLevelGroup(LevelGroup levelGroup)
        {
            var levelDatas = levelGroup.levelDatas;
            var levelCount = levelDatas.Count;

            var scaleY = CalculateStatusObjectScale(levelCount);
            var placePosition = GetPlacePosition(scaleY);

            foreach (var levelData in levelDatas)
            {
                var color = GetLevelColor(levelData);
                var statusObj = Instantiate(levelStatusPrefab, placePosition, Quaternion.identity, transform);

                var scale = statusObj.transform.localScale;
                statusObj.transform.localScale = new Vector3(scale.x, scaleY, scale.z);
                statusObj.color = color;
                placePosition.y -= scaleY;
            }
        }

        private Vector2 GetPlacePosition(float scaleY)
        {
            var bounds = levelStatusPrefab.bounds;
            var placePosition = placeTarget.position;
            var extents = bounds.extents;
            return new Vector2(placePosition.x + extents.x, placePosition.y - extents.y * scaleY);
        }

        private float CalculateStatusObjectScale(int levelCount)
        {
            var height = 2 * levelStatusPrefab.bounds.extents.y;
            var availableWorldHeight = Mathf.Abs(placeTarget.position.y - endPlaceTarget.position.y);
            return availableWorldHeight / levelCount / height;
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