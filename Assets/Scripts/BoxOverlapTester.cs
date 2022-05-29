using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxOverlapTester : MonoBehaviour
{
    [SerializeField] private Vector2 colliderSize = new Vector2(1, 1);
    [Space] [SerializeField] private LayerMask collideWith;
    [SerializeField] private int overlapBufferSize = 8;
    [SerializeField] private bool autoTest = true;
    [SerializeField] private Color debugColor = new Color(0, 1, 0, 0.4f);
    [SerializeField] private Color debugColorWhenDisabled = new Color(0, 1, 0, 0.4f);

    private Collider2D[] _overlapBuffer;
    private int _overlapCount;

    public event OverlapCallback Overlap;

    private void Awake()
    {
        _overlapBuffer = new Collider2D[overlapBufferSize];
    }

    private void FixedUpdate()
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

    private void OnDrawGizmos()
    {
        var color = enabled ? debugColor : debugColorWhenDisabled;
        GizmosHelper.PushColor(color);
        GizmosHelper.PushMatrix(transform.localToWorldMatrix);

        Gizmos.DrawCube(Vector3.zero, colliderSize);

        GizmosHelper.PopColor();
        GizmosHelper.PopMatrix();
    }

    public bool TestForOverlap()
    {
        var angle = GetOrientationAngle();
        var size = GetScaledSize();
        _overlapCount = Physics2D.OverlapBoxNonAlloc(transform.position,
            size, angle, _overlapBuffer, collideWith);

        return _overlapCount > 0;
    }

    public IEnumerable<Collider2D> GetLastSuccessfulTestOverlaps()
    {
        return _overlapBuffer.Take(_overlapCount);
    }

    public Vector2 GetScaledSize()
    {
        var scale = transform.localScale;
        return new Vector2(colliderSize.x * scale.x, colliderSize.y * scale.y);
    }

    public float GetOrientationAngle()
    {
        return transform.eulerAngles.z;
    }
}