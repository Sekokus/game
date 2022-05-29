using System.Collections;
using UnityEngine;

namespace Player
{
    public class AttackModule : PlayerModule
    {
        [SerializeField] private Hitbox hitbox;
        [SerializeField] private GameObject sprite;

        private void Start()
        {
            Core.Input.AttackAction += OnAttackAction;
            hitbox.HitDamageable += OnHitDamageable;
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
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            PushRestrictions(PlayerActionType.Jump, PlayerActionType.Dash, PlayerActionType.Attack);

            Core.Animator.SetTrigger("attack");
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.1f);
            
            hitbox.enabled = true;
            sprite.SetActive(true);
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.1f);

            sprite.SetActive(false);
            hitbox.enabled = false;

            PopRestrictions();
        }
    }
}