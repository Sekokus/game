using System;
using System.Collections;
using UnityEngine;

namespace Utilities
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

        public CoroutineContext RunAfter(Action action, float time)
        {
            var coroutine = StartCoroutine(RunAfterRoutine(action, time));
            return new CoroutineContext(this, coroutine);
        }

        public CoroutineContext RunAfter(Action action, Coroutine previousCoroutine)
        {
            var coroutine = StartCoroutine(RunAfterRoutine(action, previousCoroutine));
            return new CoroutineContext(this, coroutine);
        }

        public CoroutineContext Run(IEnumerator routine)
        {
            var coroutine = StartCoroutine(routine);
            return new CoroutineContext(this, coroutine);
        }
        
        private static IEnumerator RunAfterRoutine(Action action, float time)
        {
            yield return new WaitForSeconds(time);
            action();
        }
        
        private static IEnumerator RunAfterRoutine(Action action, Coroutine coroutine)
        {
            yield return coroutine;
            action();
        }
    }
}