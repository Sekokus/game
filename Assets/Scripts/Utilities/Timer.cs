using System;
using UnityEngine;

namespace Utilities
{
    public class Timer
    {
        public event Action Timeout;
        private float _duration;
        private float _timeRemained;
        private bool _isRunning;
        public bool IsRepeating { get; set; }
        public bool IsPaused { get; set; }

        public void Start(float time, bool repeating = false)
        {
            _duration = time;
            _isRunning = true;
            _timeRemained = time;
            IsRepeating = repeating;
        }

        public void Tick(float deltaTime)
        {
            if (!_isRunning || IsPaused)
            {
                return;
            }

            _timeRemained = Mathf.Max(_timeRemained - deltaTime, 0);
            if (Mathf.Approximately(_timeRemained, 0))
            {
                OnTimeout();
            }
        }

        private void OnTimeout()
        {
            Timeout?.Invoke();

            _isRunning = IsRepeating;
            _timeRemained = _duration;
        }

        public static Timer Start(float time, Action timeout, bool repeating = false)
        {
            var timer = new Timer();
            timer.Timeout += timeout;
            timer.Start(time, repeating);

            return timer;
        }

        public void Reset()
        {
            _isRunning = false;
            _timeRemained = 0;
        }
    }
}