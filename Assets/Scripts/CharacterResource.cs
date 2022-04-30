using System;
using UnityEngine;

public class CharacterResource
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

    public event Action<CharacterResource> OnValueChanged;

    public CharacterResource(float maxValue)
    {
        MaxValue = maxValue;
        Value = maxValue;
    }

    public void Restore(float amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Trying to restore negative amount of resource");
            return;
        }
        Value += amount;
    }

    public void Spend(float amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Trying to spend negative amount of resource");
            return;
        }
        Value -= amount;
    }
}