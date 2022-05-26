using System;
using UnityEngine;

namespace Player
{
    public class Resource
    {
        private float _value;

        public float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, 0, MaxValue);
                OnValueChanged?.Invoke(this);
            }
        }

        public float MaxValue { get; }

        public bool IsDepleted => Mathf.Approximately(Value, 0);

        public event Action<Resource> OnValueChanged;

        public Resource(float maxValue)
        {
            MaxValue = maxValue;
            Value = maxValue;
        }

        public void Restore(float amount)
        {
            if (amount <= 0)
            {
                return;
            }
            Value += amount;
        }

        public void Spend(float amount)
        {
            if (amount <= 0)
            {
                return;
            }
            Value -= amount;
        }
    }
}