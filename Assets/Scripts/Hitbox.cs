using System;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Collider2D[] ignored;
    [SerializeField] private BoxOverlapTester overlapTester;
    private HashSet<Collider2D> _ignored;

    public BoxOverlapTester OverlapTester => overlapTester;
    
    private void Awake()
    {
        _ignored = new HashSet<Collider2D>(ignored);
        overlapTester.Overlap += OnOverlap;
    }

    private void OnDisable()
    {
        overlapTester.enabled = false;
    }

    private void OnEnable()
    {
        overlapTester.enabled = true;
    }

    private void OnValidate()
    {
        overlapTester.enabled = enabled;
    }

    private void OnOverlap(IReadOnlyList<Collider2D> overlaps)
    {
        foreach (var overlap in overlaps)
        {
            ProcessOverlap(overlap);
        }
    }

    public event Action<Hurtbox> HitDamageable;
    public event Action<Collider2D> HitNonDamageable;

    private void ProcessOverlap(Collider2D overlap)
    {
        if (_ignored.Contains(overlap))
        {
            return;
        }

        if (!overlap.TryGetComponent(out Hurtbox hurtbox))
        {
            HitNonDamageable?.Invoke(overlap);
            return;
        }

        var inflicted = hurtbox.ReceiveHit(this);
        if (!inflicted)
        {
            return;
        }

        HitDamageable?.Invoke(hurtbox);
    }
}