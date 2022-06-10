using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class LevelGoalCounterText : MonoBehaviour
    {
        [FormerlySerializedAs("textMesh")] [SerializeField] private TextMeshProUGUI countTextMesh; 
        [SerializeField] private TextMeshProUGUI typeTextMesh;
        private LevelGoalCounter _counter;

        private void Awake()
        {
            _counter = Container.Get<LevelGoalCounter>();

            _counter.ValueChanged += OnValueChanged;
            typeTextMesh.text = _counter.Type.ToString();
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            var currentCount = _counter.CurrentCount;
            var minCount = _counter.MinCount;
            var maxCount = _counter.MaxCount;

            if (currentCount < minCount)
            {
                countTextMesh.text = WithColor(currentCount.ToString(), "red") + "/" + maxCount;
            }
            else if (currentCount < maxCount)
            {
                countTextMesh.text = currentCount + "/" + maxCount;
            }
            else
            {
                countTextMesh.text = WithColor(currentCount + "/" + maxCount, "green");
            }
        }

        private static string WithColor(string text, string color)
        {
            return $"<color={color}>{text}</color>";
        }
    }
}