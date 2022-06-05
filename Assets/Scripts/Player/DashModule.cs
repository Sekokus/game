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

        public event Action<float> DashStarted;
        public event Action<float> DashEnded;
        public event Action<Vector2> DashFrameStart;

        private bool _isDashing;
        
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
        }

        private void OnHitReceived(Hitbox obj)
        {
            if (_isDashing)
            {
                EndDashEarly();
            }
        }

        private void EndDashEarly()
        {
            StopAllCoroutines();
            OnDashEnded();
        }

        private void OnDashAction(bool pressed)
        {
            if (!pressed || !Core.CanPerform(PlayerRestrictions.Dash))
            {
                return;
            }

            if (!TrySpendDashCharge())
            {
                return;
            }

            var direction = GetWorldMousePosition() - Core.Rigidbody.position;
            RotateTowardsDashDirection(direction);

            StartCoroutine(DashCoroutine(direction.normalized));

            void RotateTowardsDashDirection(Vector2 dashDirection)
            {
                Core.Transform.eulerAngles = new Vector3(0, dashDirection.x > 0 ? 0 : 180, 0);
            }
        }

        private Vector2 GetWorldMousePosition()
        {
            var screenPosition = Mouse.current.position.ReadValue();
            return Core.CameraContainer.Camera.ScreenToWorldPoint(screenPosition);
        }

        private IEnumerator DashCoroutine(Vector2 direction)
        {
            PushRestrictions(PlayerRestrictions.Move, PlayerRestrictions.Attack, PlayerRestrictions.Jump,
                PlayerRestrictions.Dash);

            _isDashing = true;
            Core.Velocity = Vector2.zero;
            Core.CameraContainer.Effects.PlayDashEffect(dashFrames * Time.fixedDeltaTime);

            DashStarted?.Invoke(dashStartDelay);

            for (var i = 1; i <= dashFrames; i++)
            {
                var startPosition = Core.Rigidbody.position;
                DashFrameStart?.Invoke(startPosition);

                var distance = dashDistance / dashFrames;
                var expectedEndPosition = startPosition + direction * distance;

                Core.Rigidbody.MovePosition(expectedEndPosition);

                yield return new WaitForFixedUpdate();
            }

            ApplyEndMomentum(direction);
            OnDashEnded();
        }

        private void ApplyEndMomentum(Vector2 direction)
        {
            Core.Velocity = direction * endMomentumMultiplier;
        }

        private void OnDashEnded()
        {
            _isDashing = false;
            DashEnded?.Invoke(dashEndDelay);
            PopRestrictions();
        }
    }
}