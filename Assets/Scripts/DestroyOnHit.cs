using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private float destroyDelay = 0.5f;
    private bool _hitReceived;

    private void Reset()
    {
        hurtbox = GetComponentInChildren<Hurtbox>();
    }

    private void Awake()
    {
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
