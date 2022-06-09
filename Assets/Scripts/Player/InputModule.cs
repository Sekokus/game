using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputModule : PlayerModule
    {
        private GameState _gameState;

        protected override void Awake()
        {
            base.Awake();
            Construct();
        }

        private void Construct()
        {
            _gameState = Container.Get<GameState>();
        }

        private InputBindings _bindings;

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
            if (_gameState.CurrentState == GameStateType.MenuOpened || context.performed)
            {
                return;
            }

            JumpAction?.Invoke(context.action.IsPressed());
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (_gameState.CurrentState == GameStateType.MenuOpened || context.performed)
            {
                return;
            }

            DashAction?.Invoke(context.action.IsPressed());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (_gameState.CurrentState != GameStateType.Default || context.performed)
            {
                return;
            }

            AttackAction?.Invoke(context.action.IsPressed());
        }

        public void OnCollect(InputAction.CallbackContext context)
        {
            if (_gameState.CurrentState != GameStateType.Default || context.performed)
            {
                return;
            }

            if (context.action.IsPressed())
            {
                _gameState.PostTryCollect();
            }
        }
    }
}