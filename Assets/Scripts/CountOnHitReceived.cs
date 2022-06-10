using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CountOnHitReceived : MonoBehaviour
    {
        private LevelGoalCounter _levelGoalCounter;

        [SerializeField] private Hurtbox _hurtbox;

        private void Awake()
        {
            _levelGoalCounter = Container.Get<LevelGoalCounter>();
        }
        
        private void OnEnable()
        {
            _hurtbox.HitReceived += OnHitReceived;
        }

        private void OnDisable()
        {
            _hurtbox.HitReceived -= OnHitReceived;
        }

        private void OnHitReceived(Hitbox hitbox)
        {
            _levelGoalCounter.IncrementCounter(CountType.Enemies);
        }
    }
}