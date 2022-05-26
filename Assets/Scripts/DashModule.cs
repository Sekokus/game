﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sekokus
{
    public class DashModule : PlayerModule
    {
        [SerializeField, Min(0)] private float dashDistance = 3f;
        [SerializeField, Min(0)] private float dashStartDelay = 0.1f;
        [SerializeField, Min(0)] private float dashEndDelay = 0.1f;
        [SerializeField, Min(1)] private int dashFrames = 7;

        public event Action<float> DashStarted;
        public event Action<float> DashEnded;
        public event Action<Vector2> DashFrameStart;

        private bool TrySpendDashCharge()
        {
            var resource = Core.Resources.DashCharges;
            if (resource.Value >= 1)
            {
                resource.Spend(1);
                return true;
            }

            return false;
        }

        private void Start()
        {
            Core.Input.DashAction += OnDashAction;
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
            StartCoroutine(DashCoroutine(direction.normalized));
        }

        private static Vector2 GetWorldMousePosition()
        {
            var screenPosition = Mouse.current.position.ReadValue();
            return Camera.main!.ScreenToWorldPoint(screenPosition);
        }

        private IEnumerator DashCoroutine(Vector2 direction)
        {
            PushRestrictions(PlayerActionType.Move, PlayerActionType.Attack, PlayerActionType.Jump, PlayerActionType.Dash);

            Core.Velocity.y = 0;
            Core.Rigidbody.velocity = Vector2.zero;

            DashStarted?.Invoke(dashStartDelay);

            yield return new WaitForSeconds(dashStartDelay);

            for (int i = 1; i <= dashFrames; i++)
            {
                var startPosition = Core.Rigidbody.position;
                DashFrameStart?.Invoke(startPosition);

                var distance = dashDistance / dashFrames;
                var expectedEndPosition = startPosition + direction * distance;

                Core.Rigidbody.MovePosition(expectedEndPosition);
                yield return new WaitForFixedUpdate();
            }

            DashEnded?.Invoke(dashEndDelay);
            yield return new WaitForSeconds(dashEndDelay);


            PopRestrictions();
        }
    }
}