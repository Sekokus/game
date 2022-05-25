using System;
using UnityEngine;

namespace Sekokus.Utilities
{
    public class Timer
    {
        public event Action Timeout;
        private float _timeRemained;
        private bool _isRunning;

        public void Start(float time)
        {
            _isRunning = true;
            _timeRemained = time;
        }

        public void Tick(float deltaTime)
        {
            if (!_isRunning)
            {
                return;
            }

            _timeRemained = Mathf.Max(_timeRemained - deltaTime, 0);
            if (!Mathf.Approximately(_timeRemained, 0))
            {
                return;
            }
            
            _isRunning = false;
            Timeout?.Invoke();
        }

        public static Timer Start(float time, Action timeout)
        {
            var timer = new Timer();
            timer.Timeout += timeout;
            timer.Start(time);
            
            return timer;
        }

        public void Reset()
        {
            _isRunning = false;
            _timeRemained = 0;
        }
    }
}