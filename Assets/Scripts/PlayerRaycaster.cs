using System;
using System.Linq;
using Player;
using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    [SerializeField] private int contactBufferSize = 8;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool autoTest = true;
    [SerializeField] private bool respectObstacles;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private bool ignorePossibleObstacles;

    private RaycastHit2D[] _hits;
    public event Action<RaycastHit2D, PlayerCore> Hit;

    private void Awake()
    {
        _hits = new RaycastHit2D[contactBufferSize];
    }

    public (RaycastHit2D hit, PlayerCore player) Raycast(Vector2 direction)
    {
        var hitCount = Physics2D.RaycastNonAlloc(transform.position,
            direction, _hits, maxDistance);

        PlayerCore player = null;
        var playerHit = new RaycastHit2D();
        var obstacleHitDistance = float.MaxValue;

        foreach (var hit in _hits.Take(hitCount))
        {
            if (!playerHit)
            {
                player = hit.collider.GetComponentInParent<PlayerCore>();
                if (player)
                {
                    playerHit = hit;
                    continue;
                }
            }

            if (!respectObstacles)
            {
                continue;
            }

            var objectLayerMask = 1 << hit.collider.gameObject.layer;
            if ((objectLayerMask & obstaclesLayerMask) != 0)
            {
                if (ignorePossibleObstacles && hit.collider.GetComponent<CanIgnoreHit>())
                {
                    continue;
                }

                obstacleHitDistance = Mathf.Min(obstacleHitDistance, hit.distance);
            }
        }

        if (playerHit && playerHit.distance < obstacleHitDistance)
        {
            Hit?.Invoke(playerHit, player);
            return (playerHit, player);
        }

        return (new RaycastHit2D(), null);
    }

    private void FixedUpdate()
    {
        if (autoTest)
        {
            Raycast(transform.right);
        }
    }
}