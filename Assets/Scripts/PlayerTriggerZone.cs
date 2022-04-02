using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerTriggerZone : MonoBehaviour
{
    [SerializeField] private BoxCollider2D triggerCollider;
    [SerializeField] private bool debugDrawEnabled = true;
    [SerializeField] private Color debugColor = new Color(0, 0.7f, 0.2f, 0.3f);

    public Vector2 Center => (Vector2)transform.position + triggerCollider.offset;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            OnEnter(player);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        var player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            OnExit(player);
        }
    }

    protected virtual void OnEnter(PlayerController player)
    {
    }

    protected virtual void OnExit(PlayerController player)
    {
    }

    protected virtual void OnDrawGizmos()
    {
        if (triggerCollider == null || !debugDrawEnabled)
        {
            return;
        }
        OnDebugDraw();
    }

    protected virtual void OnDebugDraw()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawCube(Center, triggerCollider.size);
    }
}
