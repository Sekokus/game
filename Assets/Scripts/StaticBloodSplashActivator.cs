using System;
using DefaultNamespace;
using UnityEngine;

public class StaticBloodSplashActivator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        GameSettings.Instance.PropertyValueChanged += OnSettingsUpdated;
        ApplySettings();
    }

    private void OnSettingsUpdated(string updatedProperty)
    {
        if (updatedProperty == nameof(GameSettings.Instance.BloodEnabled))
        {
            ApplySettings();
        }
    }

    private void OnDestroy()
    {
        GameSettings.Instance.PropertyValueChanged -= OnSettingsUpdated;
    }

    private void ApplySettings()
    {
        var bloodEnabled = GameSettings.Instance.BloodEnabled;
        spriteRenderer.enabled = bloodEnabled;
    }
}