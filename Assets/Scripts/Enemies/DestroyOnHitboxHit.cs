using UnityEngine;

namespace Enemies
{
    public class DestroyOnHitboxHit : MonoBehaviour
    {
        [SerializeField] private DestructionHandler destructionHandler;
        [SerializeField] private Hitbox hitbox;

        private void Reset()
        {
            destructionHandler = GetComponent<DestructionHandler>();
            hitbox = GetComponentInChildren<Hitbox>();
        }

        private void Awake()
        {
            hitbox.HitDamageable += _ => Destroy();
            hitbox.HitNonDamageable += _ => Destroy();
        }

        private void Destroy() => destructionHandler.Destroy();
    }
}