using System;
using UnityEngine;

namespace Sekokus
{
    public class JumpModule : PlayerModule
    {
        [Serializable]
        private class JumpInfo
        {
            [field: SerializeField, Min(0)] public float StartVelocity { get; private set; }
            [field: SerializeField, Range(0, 1)] public float AbortFactor { get; private set; }
        }

        [Header("Jump properties")]
        [SerializeField, Range(0, 0.3f)] private float coyoteTime = 0.08f;
        [SerializeField] private JumpInfo[] jumpInfo;

        public int MaxJumpCount { get; private set; }

        private int _availableJumpCount = 0;

        private Timer _coyoteTimer;

        private JumpInfo _currentJump;
        private bool _isFalling;

        private void Start()
        {
            MaxJumpCount = jumpInfo.Length;
            Core.Movement.Landed += OnLanded;
            Core.Movement.TookOffGround += OnTookOffGround;
            Core.Input.JumpAction += OnJumpAction;
            _coyoteTimer = new Timer();
            _coyoteTimer.Timeout += OnCoyoteTimerTimeout;
        }

        private void OnCoyoteTimerTimeout()
        {
            if (_currentJump == null)
            {
                _availableJumpCount--;
            }
        }

        private void OnTookOffGround()
        {
            if (_currentJump == null)
            {
                _coyoteTimer.Start(coyoteTime);
            }
        }

        private void OnJumpAction(bool pressed)
        {
            if (!Core.CanPerform(PlayerActionType.Jump))
            {
                return;
            }

            if (HasAvailableJumps && pressed)
            {
                Jump();
            }
            else if (_currentJump != null && !pressed)
            {
                AbortJump();
            }
        }

        private void OnLanded()
        {
            _availableJumpCount = MaxJumpCount;
            _coyoteTimer.Reset();
            _currentJump = null;
        }

        public void RestoreOneJump()
        {
            _availableJumpCount = Mathf.Min(_availableJumpCount + 1, MaxJumpCount);
        }

        private void FixedUpdate()
        {
            _coyoteTimer.Tick(Time.fixedDeltaTime);

            if (_currentJump != null)
            {
                _isFalling = Core.Velocity.y < 0;
            }
        }

        private bool HasAvailableJumps => _availableJumpCount > 0;

        private JumpInfo GetNextJump()
        {
            return jumpInfo[MaxJumpCount - _availableJumpCount];
        }

        private void Jump()
        {
            _currentJump = GetNextJump();
            _isFalling = false;

            _availableJumpCount--;

            Core.Velocity.y = _currentJump.StartVelocity;
        }

        private void AbortJump()
        {
            if (_isFalling)
            {
                return;
            }

            Core.Velocity.y *= _currentJump.AbortFactor;
        }
    }
}