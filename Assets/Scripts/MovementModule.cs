using System;
using System.Linq;
using UnityEngine;

namespace Sekokus
{
    public class MovementModule : PlayerModule
    {
        [Header("Velocity properties")]
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float minVerticalVelocity = -20f;
        [SerializeField, Min(0)] private float speed = 4f;
        [SerializeField, Range(0, 1)] private float airAccelerationFactor = 0.6f;

        [Space]
        [Header("Contact checking properties")]
        [SerializeField] private LayerMask contactCheckMask;
        [SerializeField, Range(0, 1)] private float minContactAngle = 0.7f;
        [SerializeField, Range(0, 0.2f)] private float contactRaycastSizeAndDistance = 0.05f;
        [SerializeField, Range(0, 0.2f)] private float contactPositionEpsilon = 0.025f;
        [SerializeField, Min(1)] private int contactsBufferSize = 6;

        [Space]
        [Header("Other")]
        [SerializeField] private bool isSpriteInitiallyFlipped;

        [SerializeField] private float cameraFollowSmoothness;

        private RaycastHit2D[] _contacts;
        private Vector2 _startPosition;

        public event Action Landed;
        public event Action TookOffGround;

        public bool IsGrounded { get; private set; }

        private Camera _camera;

        private void Start()
        {
            _contacts = new RaycastHit2D[contactsBufferSize];
            _startPosition = Core.Transform.position;
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            CheckContacts();

            if (Core.CanPerform(PlayerActionType.Move))
            {
                ApplyMoveInput();
                ApplyGravity();
                ApplyVelocityToRigidbody();
            }

            MoveCamera();
        }

        private void MoveCamera()
        {
            var camera = (Vector2)_camera.transform.position;
            var player = Core.Rigidbody.position;

            var newCameraPos = Vector2.Lerp(camera, player, cameraFollowSmoothness);
            _camera.transform.position = new Vector3(newCameraPos.x, newCameraPos.y, _camera.transform.position.z);
        }

        private void ApplyMoveInput()
        {
            Walk(Core.Input.MoveInput.x);
        }

        private void ApplyGravity()
        {
            Core.Velocity.y += gravity * Time.fixedDeltaTime;
        }

        private void ApplyVelocityToRigidbody()
        {
            if (Core.Velocity.y < minVerticalVelocity)
            {
                Core.Velocity.y = minVerticalVelocity;
            }
            Core.Rigidbody.velocity = Core.Velocity;
        }

        private void Walk(float direction)
        {
            Core.Velocity.x = speed * direction;
            if (!IsGrounded)
            {
                Core.Velocity.x *= airAccelerationFactor;
            }

            if (Mathf.Abs(direction) > 0)
            {
                LookTo(direction);
            }
        }

        private void LookTo(float direction)
        {
            var euler = Core.Transform.eulerAngles;
            euler.y = (direction > 0) != isSpriteInitiallyFlipped ? 180 : 0;
            Core.Transform.eulerAngles = euler;
        }

        private void CheckContacts()
        {
            if (Core.Velocity.y < 0)
            {
                var hasContact = CheckBoundsContact(ContactCheckDirection.Down);
                if (IsGrounded && !hasContact)
                {
                    TookOffGround?.Invoke();
                }
                else if (!IsGrounded && hasContact)
                {
                    Landed?.Invoke();
                }

                IsGrounded = hasContact;
                if (IsGrounded)
                {
                    Core.Velocity.y = 0;
                }
            }

            if (Core.Velocity.y < 0)
            {
                return;
            }

            if (CheckBoundsContact(ContactCheckDirection.Up))
            {
                Core.Velocity.y = 0;
            }
        }

        private enum ContactCheckDirection
        {
            Up, Down
        }

        private Bounds GetActualColliderBounds()
        {
            var rawBounds = Core.Collider.bounds;
            return new Bounds(rawBounds.center, rawBounds.size + Vector3.one * (Core.Collider.edgeRadius * 2));
        }

        private bool CheckBoundsContact(ContactCheckDirection direction)
        {
            var bounds = GetActualColliderBounds();
            var castOrigin = direction == ContactCheckDirection.Down ?
                new Vector2(bounds.min.x + bounds.extents.x, bounds.min.y) :
                new Vector2(bounds.max.x - bounds.extents.x, bounds.max.y);
            var castDirection = direction == ContactCheckDirection.Down ? Vector2.down : Vector2.up;

            var contactCount = Physics2D.BoxCastNonAlloc(castOrigin,
                new Vector2(bounds.size.x, contactRaycastSizeAndDistance),
                0, castDirection,
                _contacts, contactRaycastSizeAndDistance, contactCheckMask);
            return _contacts
                .Take(contactCount)
                .Where(contact =>
                    (contact.normal.y >= minContactAngle && direction == ContactCheckDirection.Down) ||
                    (contact.normal.y <= -minContactAngle && direction == ContactCheckDirection.Up))
                .Any(contact => Mathf.Abs(contact.point.y - castOrigin.y) < contactPositionEpsilon);
        }
    }
}