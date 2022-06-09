using UnityEngine;

public class CircleOverlapTester : OverlapTester
{
    [SerializeField] private float colliderRadius = 1;

    public float GetScaledSize()
    {
        var scale = transform.lossyScale;
        return Mathf.Max(scale.x, scale.y) * colliderRadius;
    }

    public override bool TestForOverlap()
    {
        var size = GetScaledSize();
        OverlapCount = Physics2D.OverlapCircleNonAlloc(transform.position,
            size, OverlapBuffer, CollideWith);

        return OverlapCount > 0;
    }

    private void OnDrawGizmos()
    {
        var color = enabled ? DebugColor : DebugColorWhenDisabled;
        GizmosHelper.PushColor(color);

        Gizmos.DrawSphere(transform.position, GetScaledSize());

        GizmosHelper.PopColor();
    }
}