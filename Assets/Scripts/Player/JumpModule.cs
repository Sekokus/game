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

        [SerializeField] private float inputWaitTime = 1;
        
        [SerializeField] private JumpInfo[] jumpInfo;

        public int MaxJumpCount { get; private set; }

        private int _availableJumpCount = 0;

        private Timer _coyoteTimer;

        private TimedTrigger _inputWaitTrigger = new TimedTrigger();

        private JumpInfo _currentJump;
        private bool _isFalling;
        private bool _waitingForAnimationFrame;
        private bool _waitingForJumpAbort;

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
            if (_currentJump != null)
            {
                Core.Velocity.y = _currentJump.StartVelocity;
            }

            ResetWaitingForJumpFrame();
        }

        private void ResetWaitingForJumpFrame()
        {
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
            if (_currentJump != null && !pressed)
            {
                _waitingForJumpAbort = true;
            }
            
            if (_waitingForAnimationFrame || !pressed)
            {
                return;
            }
            
            if (Core.CanPerform(PlayerActionType.Jump) && HasAvailableJumps)
            {
                Jump();
            }
            else
            {
                _inputWaitTrigger.SetFor(inputWaitTime);
            }
        }

        private void OnLanded()
        {
            _availableJumpCount = MaxJumpCount;
            _coyoteTimer.Reset();
            _currentJump = null;
            ResetWaitingForJumpFrame();
        }

        public void RestoreOneJump()
        {
            _availableJumpCount = Mathf.Min(_availableJumpCount + 1, MaxJumpCount);
        }

        private void Update()
        {
            _coyoteTimer.Tick(Time.deltaTime);
            _inputWaitTrigger.Tick(Time.deltaTime);

            if (_inputWaitTrigger.IsSet && HasAvailableJumps && Core.CanPerform(PlayerActionType.Jump))
            {
                Jump();
                _inputWaitTrigger.Reset();
                return;
            }
            
            if (_currentJump != null)
            {
                _isFalling = Core.Velocity.y < 0;
            }

            if (!_waitingForAnimationFrame && _waitingForJumpAbort)
            {
                _waitingForJumpAbort = false;
                AbortJump();
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
            if (_currentJump != null)
            {
                Core.Velocity.y *= _currentJump.AbortFactor;
            }
        }
    }
}