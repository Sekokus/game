using UnityEngine;

[RequireComponent(typeof(Hurtbox))]
public class DestroyOnHit : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.5f;
    private bool _hitReceived;

    private void Awake()
    {
        var hurtbox = GetComponent<Hurtbox>();
        hurtbox.HitReceived += OnHitReceived;
    }

    private void OnHitReceived(Hitbox obj)
    {
        if (_hitReceived)
        {
            return;
        }

        _hitReceived = true;
        Destroy(gameObject, destroyDelay);
    }
}
