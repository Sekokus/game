using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sekokus
{
    public class InputModule : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public event Action<bool> JumpAction;
        public event Action<bool> DashAction;

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            JumpAction?.Invoke(context.action.IsPressed());
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            DashAction?.Invoke(context.action.IsPressed());
        }
    }
}