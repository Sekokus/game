using System;
using UnityEngine;
using Utilities;

namespace Player
{
    public class JumpModule : PlayerModule
    {
        [Serializable]
        private class JumpInfo
        {
            [field: SerializeField]
            [field: Min(0)]
            public float StartVelocity { get; private set; }

            [field: SerializeField]
            [field: Range(0, 1)]
            public float AbortFactor { get; private set; }
        }

        [Header("Jump properties")] [SerializeField] [Range(0, 0.3f)]
        private float coyoteTime = 0.08f;

        [SerializeField] private JumpInfo[] jumpInfo;

        public int MaxJumpCount { get; private set; }

        private int _availableJumpCount = 0;

        private Timer _coyoteTimer;

        private JumpInfo _currentJump;
        private bool _isFalling;
        private bool _waitingForAnimationFrame;

        private void Start()
        {
            MaxJumpCount = jumpInfo.Length;
            
            _coyoteTimer = new Timer();
            _coyoteTimer.Timeout += OnCoyoteTimerTimeout;
        }

        private void OnEnable()
        {
            Core.Movement.Landed += OnLanded;
            Core.Movement.TookOffGround += OnTookOffGround;
            Core.Input.JumpAction += OnJumpAction;
            Core.AnimationEvents.JumpFrame += OnJumpFrame;
        }

        private void OnDisable()
        {
            Core.Movement.Landed -= OnLanded;
            Core.Movement.TookOffGround -= OnTookOffGround;
            Core.Input.JumpAction -= OnJumpAction;
            Core.AnimationEvents.JumpFrame -= OnJumpFrame;
        }

        private void OnJumpFrame()
        {
            Core.Velocity.y = _currentJump.StartVelocity;
            _waitingForAnimationFrame = false;
            Core.Animator.SetBool("waiting-for-jump-frame", false);
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
            if (_waitingForAnimationFrame)
            {
                return;
            }
            
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
            _waitingForAnimationFrame = true;

            _availableJumpCount--;
            //Core.Animator.SetTrigger("jump");
            Core.Animator.SetBool("waiting-for-jump-frame", true);
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