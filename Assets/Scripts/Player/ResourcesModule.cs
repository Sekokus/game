using UnityEngine;

namespace Player
{
    public class ResourcesModule : PlayerModule
    {
        public Resource DashCharges { get; private set; }
        public Resource Health { get; private set; }

        [SerializeField] private float dashPassiveRechargeSpeed;

        protected override void Awake()
        {
            base.Awake();
            DashCharges = new Resource(4);
            Health = new Resource(4);

            var hurtbox = GetComponentInChildren<Hurtbox>();
            hurtbox.HitReceived += OnHitReceived;
        }

        private void OnHitReceived(Hitbox obj)
        {
            Health.Spend(1);
        }

        private void FixedUpdate()
        {
            DashCharges.Restore(dashPassiveRechargeSpeed * Time.fixedDeltaTime);
        }
    }
}