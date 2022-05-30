using System;

namespace DefaultNamespace
{
    public class LevelGoalCounter
    {
        public int RequiredCount { get; private set; }
        public int CurrentCount { get; private set; }

        public event Action ValueChanged;
        public event Action ReachedRequiredCount;


        public void SetRequiredCount(int count)
        {
            RequiredCount = count;
        }

        public void IncrementCounter()
        {
            CurrentCount++;
            ValueChanged?.Invoke();
            if (CurrentCount == RequiredCount)
            {
                ReachedRequiredCount?.Invoke();
            }
        }

        public void ResetCounter()
        {
            CurrentCount = 0;
        }
    }
}