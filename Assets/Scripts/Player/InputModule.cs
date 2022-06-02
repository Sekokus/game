using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputModule : PlayerModule
    {
        private PauseService _pauseService;
        private PauseObserver _pauseObserver;
        private GameEvents _gameEvents;

        protected override void Awake()
        {
            base.Awake();
            Construct();
        }

        private void Construct()
        {
            _gameEvents = Container.Get<GameEvents>();
            _pauseService = Container.Get<PauseService>();
            _pauseObserver = _pauseService.GetObserver(PauseSource.Any);
        }

        public Vector2 MoveInput { get; private set; }

        public event Action<bool> AttackAction;
        public event Action<bool> JumpAction;
        public event Action<bool> DashAction;

        public void OnMove(InputAction.CallbackContext context)
        {
            if (_pauseObserver.IsPaused)
            {
                return;
            }

            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (_pauseObserver.IsPaused || context.performed)
            {
                return;
            }

            JumpAction?.Invoke(context.action.IsPressed());
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (_pauseObserver.IsPaused || context.performed)
            {
                return;
            }

            DashAction?.Invoke(context.action.IsPressed());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (_pauseObserver.IsPaused || context.performed)
            {
                return;
            }

            AttackAction?.Invoke(context.action.IsPressed());
        }

        public void OnCollect(InputAction.CallbackContext context)
        {
            if (_pauseObserver.IsPaused || context.performed)
            {
                return;
            }
            
            if (context.action.IsPressed())
            {
                _gameEvents.PostTryCollect();
            }
        }
    }
}