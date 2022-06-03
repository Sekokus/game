using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelGoalCounter
    {
        public int MinCount { get; private set; }
        public int MaxCount { get; private set; }

        public int CurrentCount { get; private set; }

        public event Action ValueChanged;
        public event Action ReachedMinCount;
        public event Action ReachedMaxCount;


        public void SetCounts(int minCount, int maxCount)
        {
            MinCount = minCount;
            MaxCount = maxCount;
        }

        public void IncrementCounter()
        {
            CurrentCount = Mathf.Min(CurrentCount + 1, MaxCount);
            
            ValueChanged?.Invoke();
            if (CurrentCount == MinCount)
            {
                ReachedMinCount?.Invoke();
            }

            if (CurrentCount == MaxCount)
            {
                ReachedMaxCount?.Invoke();
            }
        }

        public void ResetCounter()
        {
            CurrentCount = 0;
        }
    }
}