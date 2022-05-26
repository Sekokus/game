using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sekokus
{
    public class InputModule : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public event Action<bool> AttackAction;
        public event Action<bool> JumpAction;
        public event Action<bool> DashAction;

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                return;
            }
            JumpAction?.Invoke(context.action.IsPressed());
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                return;
            }
            DashAction?.Invoke(context.action.IsPressed());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                return;
            }
            AttackAction?.Invoke(context.action.IsPressed());
        }
    }
}