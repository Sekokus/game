using UnityEngine;

namespace Enemies
{
    public class DestroyOnHitboxHit : MonoBehaviour
    {
        [SerializeField] private DestructionHandler destructionHandler;
        [SerializeField] private Hitbox hitbox;
        [SerializeField] private bool ignorePossibleHits;

        private void Reset()
        {
            destructionHandler = GetComponent<DestructionHandler>();
            hitbox = GetComponentInChildren<Hitbox>();
        }

        private void Awake()
        {
            hitbox.HitDamageable += _ => Destroy();
            hitbox.HitNonDamageable += hit =>
            {
                if (ignorePossibleHits && hit.GetComponent<ProjectilesCanPassThrough>())
                {
                    return;
                }

                Destroy();
            };
        }

        private void Destroy() => destructionHandler.Destroy();
    }
}