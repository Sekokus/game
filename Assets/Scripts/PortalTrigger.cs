using Sekokus.Player;
using UnityEngine;

public class PortalTrigger : PlayerTriggerZone
{
    [SerializeField] private Vector2 destination;

    protected override void OnEnter(PlayerCore player)
    {
        player.Rigidbody.position = destination;
    }

    protected override void OnDebugDraw()
    {
        base.OnDebugDraw();
        Gizmos.DrawLine(Center, destination);
        Gizmos.DrawSphere(destination, 0.3f);
    }
}
