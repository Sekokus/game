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
            SetText(_counter.CurrentCount, _counter.RequiredCount);
        }

        private void SetText(int currentCount, int reqCount)
        {
            textMesh.text = currentCount + "\\" + reqCount;
        }
    }
}