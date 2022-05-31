using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class TimerRunner : MonoBehaviour
    {
        private readonly List<Timer> _timers = new List<Timer>();
        
        public Timer CreateTimer(Action timeout = null)
        {
            var timer = new Timer();
            if (timeout != null)
            {
                timer.Timeout += timeout;
            }
            
            _timers.Add(timer);
            return timer;
        }

        public bool RemoveTimer(Timer timer) => _timers.Remove(timer);

        private void Update()
        {
            TickTimers(Time.deltaTime);
        }

        private void TickTimers(float deltaTime)
        {
            foreach (var timer in _timers)
            {
                timer.Tick(deltaTime);
            }
        }
    }
}