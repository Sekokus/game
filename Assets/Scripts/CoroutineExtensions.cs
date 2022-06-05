using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace DefaultNamespace
{
    public readonly struct DoEveryFrameState
    {
        public readonly float DeltaTime;
        public readonly float Fraction;
        public readonly float PassedTime;

        public DoEveryFrameState(float deltaTime, float fraction, float passedTime)
        {
            DeltaTime = deltaTime;
            Fraction = fraction;
            PassedTime = passedTime;
        }
    }

    public delegate void DoEveryFrameCallback(DoEveryFrameState state);

    public static class Do
    {
        [MustUseReturnValue("Use .Start(host)")]
        public static SmartCoroutine After(Action action, float time)
        {
            var smartCoroutine = new SmartCoroutine();
            return smartCoroutine.After(action, time);
        }

        [MustUseReturnValue("Use .Start(host)")]
        public static SmartCoroutine EveryFrameFor(DoEveryFrameCallback action, float time)
        {
            var smartCoroutine = new SmartCoroutine();
            return smartCoroutine.EveryFrameFor(action, time);
        }
    }

    public class SmartCoroutine
    {
        private readonly List<Func<IEnumerator>> _routines = new List<Func<IEnumerator>>();

        private SmartCoroutine With(Func<IEnumerator> routine)
        {
            _routines.Add(routine);
            return this;
        }

        public SmartCoroutine After(Action action, float time)
        {
            IEnumerator Wrapper()
            {
                yield return new WaitForSeconds(time);
                action();
            }

            return With(Wrapper);
        }

        public SmartCoroutine EveryFrameFor(DoEveryFrameCallback action, float time)
        {
            IEnumerator Wrapper()
            {
                var passedTime = 0f;
                var lastUpdate = Time.time;
                do
                {
                    yield return null;
                    var newUpdate = Time.time;
                    var deltaTime = newUpdate - lastUpdate;
                    lastUpdate = newUpdate;

                    passedTime = Mathf.Min(passedTime + deltaTime, time);
                    var state = new DoEveryFrameState(deltaTime, passedTime / time, passedTime);
                    action(state);
                } while (passedTime < time);
            }

            return With(Wrapper);
        }

        public Coroutine Start(MonoBehaviour host)
        {
            IEnumerator CombinedWrapper()
            {
                foreach (var routine in _routines)
                {
                    yield return routine();
                }
            }

            return host.StartCoroutine(CombinedWrapper());
        }

        public SmartCoroutine Action(Action action)
        {
            IEnumerator Wrapper()
            {
                action();
                yield break;
            }

            return With(Wrapper);
        }
    }
}