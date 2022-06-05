using UnityEngine;

namespace Player
{
    public class ResourcesModule : PlayerModule
    {
        public Resource DashCharges { get; private set; }
        public Resource Health { get; private set; }

        [SerializeField] private float dashPassiveRechargeSpeed;
        [SerializeField, Range(0, 1)] private float airboneDashRechargeFactor = 0.3f;

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
            if (Health.IsDepleted)
            {
                Core.GameEvents.PostPlayerDied();
            }
        }

        private void Update()
        {
            if (PauseObserver.IsPaused)
            {
                return;
            }

            var factor = Core.Movement.IsGrounded ? 1 : airboneDashRechargeFactor;
            DashCharges.Restore(factor * dashPassiveRechargeSpeed * Time.deltaTime);
        }
    }
}