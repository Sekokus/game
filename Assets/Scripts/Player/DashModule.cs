using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class DashModule : PlayerModule
    {
        [SerializeField] [Min(0)] private float dashDistance = 3f;
        [SerializeField] private float endMomentumMultiplier = 3f;
        [SerializeField] [Min(0)] private float dashStartDelay = 0.1f;
        [SerializeField] [Min(0)] private float dashEndDelay = 0.1f;
        [SerializeField] [Min(1)] private int dashFrames = 7;

        private CoroutineRunner _coroutineRunner;
        private CoroutineRunner.CoroutineContext _dashRoutine;
        private PauseObserver _pauseObserver;

        public event Action<float> DashStarted;
        public event Action<float> DashEnded;
        public event Action<Vector2> DashFrameStart;

        private bool TrySpendDashCharge()
        {
            var resource = Core.Resources.DashCharges;
            if (resource.Value < 1)
            {
                return false;
            }

            resource.Spend(1);
            return true;
        }

        private void Start()
        {
            Core.Input.DashAction += OnDashAction;

            var hurtbox = GetComponentInChildren<Hurtbox>();
            hurtbox.HitReceived += OnHitReceived;

            _coroutineRunner = Container.Get<CoroutineRunner>();
            _pauseObserver = Container.Get<PauseService>().GetObserver(PauseSource.Any);
        }

        private void OnHitReceived(Hitbox obj)
        {
            EndDashEarly();
        }

        private void EndDashEarly()
        {
            if (_dashRoutine == null)
            {
                return;
            }

            _dashRoutine.Stop();
            OnDashEnded();
        }

        private void OnDashAction(bool pressed)
        {
            if (!pressed || !Core.CanPerform(PlayerActionType.Dash))
            {
                return;
            }

            if (!TrySpendDashCharge())
            {
                return;
            }

            var direction = GetWorldMousePosition() - Core.Rigidbody.position;
            RotateTowardsDashDirection(direction);

            _dashRoutine = _coroutineRunner.Run(DashCoroutine(direction.normalized));

            void RotateTowardsDashDirection(Vector2 dashDirection)
            {
                Core.Transform.eulerAngles = new Vector3(0, dashDirection.x > 0 ? 0 : 180, 0);
            }
        }

        private static Vector2 GetWorldMousePosition()
        {
            var screenPosition = Mouse.current.position.ReadValue();
            return Camera.main!.ScreenToWorldPoint(screenPosition);
        }

        private IEnumerator DashCoroutine(Vector2 direction)
        {
            PushRestrictions(PlayerActionType.Move, PlayerActionType.Attack, PlayerActionType.Jump,
                PlayerActionType.Dash);

            Core.Velocity = Vector2.zero;

            DashStarted?.Invoke(dashStartDelay);

            yield return new WaitForSeconds(dashStartDelay);

            for (var i = 1; i <= dashFrames; i++)
            {
                var startPosition = Core.Rigidbody.position;
                DashFrameStart?.Invoke(startPosition);

                var distance = dashDistance / dashFrames;
                var expectedEndPosition = startPosition + direction * distance;

                Core.Rigidbody.MovePosition(expectedEndPosition);
                yield return new WaitForFixedUpdate();
                if (_pauseObserver.IsPaused)
                {
                    yield return new WaitUntil(() => _pauseObserver.IsUnpaused);
                }
            }

            OnDashEnded();
            ApplyEndMomentum(direction);
        }

        private void ApplyEndMomentum(Vector2 direction)
        {
            Core.Velocity = direction * endMomentumMultiplier;
        }

        private void OnDashEnded()
        {
            DashEnded?.Invoke(dashEndDelay);
            _coroutineRunner.RunAfter(PopRestrictions, dashEndDelay);
            _dashRoutine = null;
        }

        private void OnDisable()
        {
            _dashRoutine?.Stop();
        }
    }
}