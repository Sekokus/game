using System;
using System.Collections;
using UnityEngine;

namespace Sekokus.Utilities
{
    public class CoroutineRunner : MonoBehaviour
    {
        public class CoroutineContext
        {
            private readonly CoroutineRunner _runner;
            private readonly Coroutine _coroutine;

            public CoroutineContext(CoroutineRunner runner, Coroutine coroutine)
            {
                _coroutine = coroutine;
                _runner = runner;
            }

            public void Stop()
            {
                _runner.StopCoroutine(_coroutine);
            }

            public static implicit operator Coroutine(CoroutineContext context)
            {
                return context._coroutine;
            }
        }

        public CoroutineContext ExecuteAfter(Action action, float time)
        {
            var coroutine = StartCoroutine(ExecuteAfterRoutine(action, time));
            return new CoroutineContext(this, coroutine);
        }

        public CoroutineContext ExecuteAfter(Action action, Coroutine previousCoroutine)
        {
            var coroutine = StartCoroutine(ExecuteAfterRoutine(action, previousCoroutine));
            return new CoroutineContext(this, coroutine);
        }

        private static IEnumerator ExecuteAfterRoutine(Action action, float time)
        {
            yield return new WaitForSeconds(time);
            action();
        }
        
        private static IEnumerator ExecuteAfterRoutine(Action action, Coroutine coroutine)
        {
            yield return coroutine;
            action();
        }
    }
}