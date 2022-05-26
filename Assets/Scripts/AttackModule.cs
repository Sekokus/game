using System.Collections;
using UnityEngine;

namespace Sekokus
{
    public class AttackModule : PlayerModule
    {
        [SerializeField] private Hitbox hitbox;
        [SerializeField] private GameObject sprite;

        private void Start()
        {
            Core.Input.AttackAction += OnAttackAction;
            hitbox.HitInflicted += OnHitInflicted;
        }

        private void OnHitInflicted(Hurtbox obj)
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

            hitbox.Enable();
            sprite.SetActive(true);
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.1f);

            // TODO: тут анимация типа

            sprite.SetActive(false);
            hitbox.Disable();

            PopRestrictions();
        }
    }
}