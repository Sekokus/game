using System;
using UnityEngine;

namespace Sekokus
{
    public class ResourcesModule : PlayerModule
    {
        public CharacterResource DashCharges { get; private set; }
        public CharacterResource Health { get; private set; }

        [SerializeField] private float dashPassiveRechargeSpeed;

        protected override void Awake()
        {
            base.Awake();
            DashCharges = new CharacterResource(4);
            Health = new CharacterResource(4);
        }

        private void FixedUpdate()
        {
            DashCharges.Restore(dashPassiveRechargeSpeed * Time.fixedDeltaTime);
        }
    }
}