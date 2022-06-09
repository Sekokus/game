using System;
using UnityEngine;

public class BoxOverlapTester : OverlapTester
{
    [SerializeField] private Vector2 colliderSize = new Vector2(1, 1);

    public Vector2 GetScaledSize()
    {
        var scale = transform.lossyScale;
        return new Vector2(colliderSize.x * scale.x, colliderSize.y * scale.y);
    }

    public float GetOrientationAngle()
    {
        return transform.eulerAngles.z;
    }

    public override bool TestForOverlap()
    {
        var angle = GetOrientationAngle();
        var size = GetScaledSize();
        OverlapCount = Physics2D.OverlapBoxNonAlloc(transform.position,
            size, angle, OverlapBuffer, CollideWith);

        return OverlapCount > 0;
    }

    private void OnDrawGizmos()
    {
        var color = enabled ? DebugColor : DebugColorWhenDisabled;
        GizmosHelper.PushColor(color);
        GizmosHelper.PushMatrix(transform.localToWorldMatrix);

        Gizmos.DrawCube(Vector3.zero, colliderSize);

        GizmosHelper.PopColor();
        GizmosHelper.PopMatrix();
    }
}