using UnityEngine;

namespace Sekokus.Player
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
        }

        private void FixedUpdate()
        {
            DashCharges.Restore(dashPassiveRechargeSpeed * Time.fixedDeltaTime);
        }
    }
}