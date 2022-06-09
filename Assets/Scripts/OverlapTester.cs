using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class OverlapTester : MonoBehaviour
{
    [SerializeField] private LayerMask collideWith;
    [SerializeField] private int overlapBufferSize = 8;
    [SerializeField] private bool autoTest = true;
    [SerializeField] private Color debugColor = new Color(0, 1, 0, 0.4f);
    [SerializeField] private Color debugColorWhenDisabled = new Color(0, 1, 0, 0.4f);

    public event OverlapCallback Overlap;

    protected void Awake()
    {
        OverlapBuffer = new Collider2D[overlapBufferSize];
    }

    protected void FixedUpdate()
    {
        if (!autoTest)
        {
            return;
        }

        if (!TestForOverlap())
        {
            return;
        }

        var overlaps = GetLastSuccessfulTestOverlaps().ToArray();
        Overlap?.Invoke(overlaps);
    }

    public abstract bool TestForOverlap();

    public IEnumerable<Collider2D> GetLastSuccessfulTestOverlaps()
    {
        return OverlapBuffer.Take(OverlapCount);
    }

    [NonSerialized] protected Collider2D[] OverlapBuffer;
    [NonSerialized] protected int OverlapCount;

    public LayerMask CollideWith => collideWith;

    public int OverlapBufferSize => overlapBufferSize;

    public bool AutoTest => autoTest;

    public Color DebugColor => debugColor;

    public Color DebugColorWhenDisabled => debugColorWhenDisabled;
}