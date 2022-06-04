using UnityEngine;
using UnityEngine.InputSystem;

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
            
            if (Core.Movement.IsGrounded)
            {
                DashCharges.Restore(dashPassiveRechargeSpeed * Time.deltaTime);
            }
            
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                Core.GameEvents.PostPlayerDied();
            }
        }
    }
}