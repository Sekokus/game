using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class LinearRotation : MonoBehaviour
    {
        private enum RotationMode
        {
            Constant,
            Random
        }

        public float rotationSpeed;
        public Vector3 axis = new Vector3(0, 0, 1);

        [SerializeField] private RotationMode mode = RotationMode.Constant;

        [SerializeField] private float randomRotationMinWaitTime = 0.5f;
        [SerializeField] private float randomRotationMaxWaitTime = 1.5f;
        [SerializeField] private float randomRotationMinRotationTime = 0.5f;
        [SerializeField] private float randomRotationMaxRotationTime = 1.5f;
        [SerializeField, Range(0, 1)] private float directionChangeChance = 0.5f;

        private int _randomRotationDirection = 1;

        private void Start()
        {
            if (mode == RotationMode.Random)
            {
                InitializeRandomRotation();
            }
        }

        private void InitializeRandomRotation()
        {
            var time = Random.Range(randomRotationMinRotationTime, randomRotationMaxRotationTime);
            var waitTime = Random.Range(randomRotationMinWaitTime, randomRotationMaxWaitTime);

            Do.EveryFrameFor(state =>
                    {
                        transform.Rotate(axis,
                            _randomRotationDirection * rotationSpeed * state.DeltaTime);
                    },
                    time)
                .After(() =>
                {
                    if (Random.value < directionChangeChance)
                    {
                        _randomRotationDirection *= -1;
                    }
                }, waitTime)
                .Action(InitializeRandomRotation)
                .Start(this);
        }

        private void Update()
        {
            if (mode != RotationMode.Constant)
            {
                return;
            }

            transform.Rotate(axis, rotationSpeed * Time.deltaTime);
        }
    }
}