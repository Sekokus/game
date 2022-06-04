using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelGoalCounterText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        private LevelGoalCounter _counter;

        private void Awake()
        {
            _counter = Container.Get<LevelGoalCounter>();

            _counter.ValueChanged += OnValueChanged;
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            var currentCount = _counter.CurrentCount;
            var minCount = _counter.MinCount;
            var maxCount = _counter.MaxCount;

            if (currentCount < minCount)
            {
                textMesh.text = WithColor(currentCount.ToString(), "red") + "/" + maxCount;
            }
            else if (currentCount < maxCount)
            {
                textMesh.text = currentCount + "/" + maxCount;
            }
            else
            {
                textMesh.text = WithColor(currentCount + "/" + maxCount, "green");
            }
        }

        private static string WithColor(string text, string color)
        {
            return $"<color={color}>{text}</color>";
        }
    }
}