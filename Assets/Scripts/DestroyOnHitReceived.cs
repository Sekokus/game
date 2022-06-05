using Enemies;
using UnityEngine;

public class DestroyOnHitReceived : MonoBehaviour
{
    [SerializeField] private DestructionHandler destructionHandler;
    [SerializeField] private Hurtbox hurtbox;

    private void Reset()
    {
        destructionHandler = GetComponent<DestructionHandler>();
        hurtbox = GetComponentInChildren<Hurtbox>();
    }

    private void Awake()
    {
        hurtbox.HitReceived += OnHitReceived;
    }

    private void OnHitReceived(Hitbox obj)
    {
        destructionHandler.Destroy();
    }
}
