using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sekokus.Player
{
    public class InputModule : MonoBehaviour
    {
        private PauseService _pauseService;
        private PauseObserver _pauseObserver;

        private void Awake()
        {
            Construct();
        }

        private void Construct()
        {
            _pauseService = Container.Get<PauseService>();
            _pauseObserver = _pauseService.GetObserver(PauseSource.Any);
        }

        public Vector2 MoveInput { get; private set; }
        public event Action<bool> JumpAction;
        public event Action<bool> DashAction;

        public void OnMove(InputAction.CallbackContext context)
        {
            if (_pauseObserver.IsUnpaused)
            {
                MoveInput = context.ReadValue<Vector2>();
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (_pauseObserver.IsUnpaused)
            {
                JumpAction?.Invoke(context.action.IsPressed());
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (_pauseObserver.IsUnpaused)
            {
                DashAction?.Invoke(context.action.IsPressed());
            }
        }
    }
}