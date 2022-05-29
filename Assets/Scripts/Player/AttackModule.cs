using UnityEngine;

namespace Player
{
    public class AttackModule : PlayerModule
    {
        [SerializeField] private Hitbox hitbox;

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
            if (!pressed || !Core.CanPerform(PlayerActionType.Attack))
            {
                return;
            }

            Attack();
        }

        private void Attack()
        {
            PushRestrictions(PlayerActionType.Jump, PlayerActionType.Dash, PlayerActionType.Attack);

            Core.Animator.SetTrigger("attack");
        }
    }
}