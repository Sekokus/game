using System;
using UnityEngine;
using Utilities;

namespace Player
{
    public class AttackModule : PlayerModule
    {
        [SerializeField] private Hitbox hitbox;

        private readonly TimedTrigger _inputTrigger = new TimedTrigger();
        [SerializeField] private float inputWaitTime = 1;

        private void OnEnable()
        {
            Core.Input.AttackAction += OnAttackAction;
            hitbox.HitDamageable += OnHitDamageable;
            Core.AnimationEvents.AttackDisableFrame += OnAttackDisableFrame;
        }

        private void OnDisable()
        {
            Core.Input.AttackAction -= OnAttackAction;
            hitbox.HitDamageable -= OnHitDamageable;
            Core.AnimationEvents.AttackDisableFrame -= OnAttackDisableFrame;
        }

        private void OnAttackDisableFrame()
        {
            PopRestrictions();
        }

        private void OnHitDamageable(Hurtbox obj)
        {
            Core.Resources.DashCharges.Restore(1);
            Core.Jump.RestoreOneJump();
        }

        private void OnAttackAction(bool pressed)
        {
            if (!pressed)
            {
                return;
            }

            if (Core.CanPerform(PlayerActionType.Attack))
            {
                Attack();
            }
            else
            {
                _inputTrigger.SetFor(inputWaitTime);
            }
        }

        private void Attack()
        {
            PushRestrictions(PlayerActionType.Jump, PlayerActionType.Dash, PlayerActionType.Attack);

            Core.Animator.SetTrigger("attack");
        }

        private void Update()
        {
            if (Core.CanPerform(PlayerActionType.Attack) && _inputTrigger.IsSet)
            {
                Attack();
                _inputTrigger.Reset();
            }
            
            _inputTrigger.Tick(Time.deltaTime);
        }
    }
}