using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class AttackModule : PlayerModule
    {
        [SerializeField] private Hitbox hitbox;

        private readonly TimedTrigger _inputTrigger = new TimedTrigger();

        [SerializeField] private int killsToHeal = 4;
        [SerializeField] private float inputWaitTime = 1;
        [SerializeField] private float onHitUpMomentum = 3;
        [SerializeField] private ParticlesGroup healParticles;

        private bool _isAttacking;
        private int _kills = 0;

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
            _isAttacking = false;
            PopRestrictions();
        }

        private void OnHitDamageable(Hurtbox obj)
        {
            Core.CameraContainer.Effects.PlayHitInflictedEffect();

            Core.Resources.DashCharges.Restore(1);
            Core.Jump.RestoreOneJump();

            if (obj.RestoreHealthOnHit)
            {
                IncreaseKillCount();
            }

            if (!Core.Movement.IsGrounded)
            {
                Core.Velocity.y = onHitUpMomentum;
            }
        }

        private void IncreaseKillCount()
        {
            _kills += 1;
            if (_kills < killsToHeal)
            {
                return;
            }

            _kills = 0;
            if (Core.Resources.Health.Value < Core.Resources.Health.MaxValue)
            {
                Core.Resources.Health.Restore(1);
                healParticles.PlayOnce();
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
            PushRestrictions(PlayerRestrictions.Jump, PlayerRestrictions.Dash, PlayerRestrictions.Attack,
                PlayerRestrictions.Rotate);

            _isAttacking = true;
            Core.Animator.SetTrigger("attack");
        }

        private void LookInCursorDirection()
        {
            var cursorScreen = Mouse.current.position.ReadValue();
            var cursorWorld = Core.CameraContainer.Camera.ScreenToWorldPoint(cursorScreen);
            var direction = cursorWorld.x - Core.Transform.position.x;
            Core.Movement.LookInDirection(direction);
        }

        private void Update()
        {
            if (Core.CanPerform(PlayerRestrictions.Attack) && _inputTrigger.IsSet)
            {
                Attack();
                _inputTrigger.Reset();
            }

            _inputTrigger.Tick(Time.deltaTime);

            if (_isAttacking)
            {
                LookInCursorDirection();
            }
        }
    }
}