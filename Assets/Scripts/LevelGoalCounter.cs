using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelGoalCounter
    {
        public int MinCount { get; private set; }
        public int MaxCount { get; private set; }

        public int CurrentCount { get; private set; }
        
        public CountType Type { get; private set; }

        public event Action ValueChanged;
        public event Action ReachedMinCount;
        public event Action ReachedMaxCount;


        public void SetCounts(int minCount, int maxCount, CountType type)
        {
            MinCount = minCount;
            MaxCount = maxCount;
            Type = type;
        }

        public void IncrementCounter(CountType type)
        {
            if (type != Type)
            {
                return;
            }
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

    public enum CountType
    {
        Collectables,
        Enemies
    }
}