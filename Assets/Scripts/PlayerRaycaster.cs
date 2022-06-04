using System;
using System.Linq;
using Player;
using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    [SerializeField] private int contactBufferSize = 8;
    [SerializeField] private float maxDistance;

    private RaycastHit2D[] _hits;

    public event Action<RaycastHit2D, PlayerCore> Hit;

    private void Awake()
    {
        _hits = new RaycastHit2D[contactBufferSize];
    }

    private void FixedUpdate()
    {
        var hitCount = Physics2D.RaycastNonAlloc(transform.position,
            transform.right, _hits, maxDistance);
        foreach (var hit in _hits.Take(hitCount))
        {
            var player = hit.collider.GetComponentInParent<PlayerCore>();
            if (!player)
            {
                continue;
            }

            Hit?.Invoke(hit, player);
            break;
        }
    }
}