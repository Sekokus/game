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
        [SerializeField] private float onHitUpMomentum = 3;

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
            
            if (!Core.Movement.IsGrounded)
            {
                Core.Velocity.y = onHitUpMomentum;
            }
        }

        private void OnAttackAction(bool pressed)
        {
            if (!pressed)
            {
                return;
            }

            if (Core.CanPerform(PlayerRestrictions.Attack))
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
            PushRestrictions(PlayerRestrictions.Jump, PlayerRestrictions.Dash, PlayerRestrictions.Attack);

            Core.Animator.SetTrigger("attack");
        }

        private void Update()
        {
            if (Core.CanPerform(PlayerRestrictions.Attack) && _inputTrigger.IsSet)
            {
                Attack();
                _inputTrigger.Reset();
            }
            
            _inputTrigger.Tick(Time.deltaTime);
        }
    }
}