using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class MovementModule : PlayerModule
    {
        [Header("Velocity properties")] [SerializeField]
        private float gravity = -9.81f;

        [SerializeField] private float minVerticalVelocity = -20f;
        [SerializeField] [Min(0)] private float speed = 4f;
        [SerializeField] [Range(0, 1)] private float airAccelerationFactor = 0.6f;
        [SerializeField] [Min(0)] private float pushTime = 1;
        [SerializeField, Min(0)] private float pushForce = 8;
        [SerializeField] [Range(0, 1)] private float dampening = 0.1f;


        [Space] [Header("Contact checking properties")] [SerializeField]
        private LayerMask contactCheckMask;

        [SerializeField] private float slopeCheckDistance = 1;
        [SerializeField] [Range(0, 1)] private float minContactAngle = 0.7f;
        [SerializeField] [Range(0, 0.2f)] private float contactRaycastSizeAndDistance = 0.05f;
        [SerializeField] [Range(0, 0.2f)] private float contactPositionEpsilon = 0.025f;
        [SerializeField] [Min(1)] private int contactsBufferSize = 6;


        [Space] [Header("Other")] [SerializeField]
        private bool isSpriteInitiallyFlipped;

        [SerializeField] private float cameraFollowSmoothness;

        private RaycastHit2D[] _contacts;
        private Vector2 _startPosition;
        private CoroutineRunner _coroutineRunner;

        public event Action Landed;
        public event Action TookOffGround;

        public bool IsGrounded { get; private set; }

        private Camera _camera;

        private void Start()
        {
            _contacts = new RaycastHit2D[contactsBufferSize];
            _startPosition = Core.Transform.position;
            _camera = Camera.main;

            var hurtbox = GetComponentInChildren<Hurtbox>();
            hurtbox.HitReceived += OnHitReceived;

            _coroutineRunner = Container.Get<CoroutineRunner>();
        }

        private void OnHitReceived(Hitbox obj)
        {
            var pushDirection = GetPushDirection(obj);
            Push(pushDirection);
        }

        private Vector3 GetPushDirection(Hitbox hitbox)
        {
            return (Core.transform.position - hitbox.transform.position).normalized;
        }

        private void Push(Vector3 direction)
        {
            PushRestrictions(
                PlayerRestrictions.Jump,
                PlayerRestrictions.Attack,
                PlayerRestrictions.Move,
                PlayerRestrictions.Dash);

            var pushForceVector = direction * pushForce;
            Core.Velocity.x = Core.Velocity.x * 0.4f + pushForceVector.x;
            Core.Velocity.y += pushForceVector.y;

            _coroutineRunner.RunAfter(PopRestrictions, pushTime);
        }

        private bool _paused = false;
        [SerializeField, Range(0, 1)] private float slopeHorizontalDampening = 0.2f;

        private void FixedUpdate()
        {
            if (PauseObserver.IsPaused)
            {
                if (!_paused)
                {
                    Core.Rigidbody.Sleep();
                    _paused = true;
                }

                return;
            }

            if (_paused)
            {
                _paused = false;
                Core.Rigidbody.WakeUp();
            }

            CheckContacts();
            ApplyGravity();

            if (Core.CanPerform(PlayerRestrictions.Move))
            {
                ApplyMoveInput();
            }

            ApplySlopeCorrection();
            DampenHorizontalVelocity();
            ApplyVelocityToRigidbody();

            UpdateAnimatorValues();

            MoveCamera();
        }

        private void ApplySlopeCorrection()
        {
            if (!IsGrounded)
            {
                return;
            }

            var hit = Physics2D.Raycast(Core.Rigidbody.position,
                Vector2.down, slopeCheckDistance, contactCheckMask);

            if (!hit)
            {
                return;
            }

            if (hit.normal.y < 1 && Core.Velocity.x * hit.normal.x >= 0)
            {
                Core.Velocity.y = Mathf.Abs(Core.Velocity.x) > 1e-1 ? -hit.normal.y * 5 : 0;
                Core.Velocity.x *= 1 - slopeHorizontalDampening;
            }
        }

        private void UpdateAnimatorValues()
        {
            Core.Animator.SetBool("is-grounded", IsGrounded);
            Core.Animator.SetFloat("vertical-velocity", Core.Velocity.y);
            Core.Animator.SetBool("is-walking", Mathf.Abs(Core.Input.MoveInput.x) > 0);
        }

        private void DampenHorizontalVelocity()
        {
            Core.Velocity.x *= 1 - dampening;
        }

        private void MoveCamera()
        {
            var cameraPosition = _camera.transform.position;
            var player = Core.Rigidbody.position;

            var newCameraPos = Vector2.Lerp(cameraPosition, player, cameraFollowSmoothness);
            _camera.transform.position = new Vector3(newCameraPos.x, newCameraPos.y, cameraPosition.z);
        }

        private void ApplyMoveInput()
        {
            var input = Core.Input.MoveInput;
            if (input.y < 0)
            {
                Core.PostDownAction();
            }

            if (Mathf.Abs(input.x) > 0)
            {
                Walk(input.x);
            }
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

            if (Core.CanPerform(PlayerRestrictions.Rotate))
            {
                LookToCursor();
            }
        }

        private void LookToCursor()
        {
            var cursorScreen = Mouse.current.position.ReadValue();
            var cursorWorld = _camera.ScreenToWorldPoint(cursorScreen);
            var direction = cursorWorld.x - Core.Transform.position.x;
            var euler = Core.Transform.eulerAngles;
            euler.y = direction > 0 == isSpriteInitiallyFlipped ? 0 : 180;
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

            if (Core.Velocity.y <= 0)
            {
                return;
            }

            IsGrounded = false;
            if (CheckBoundsContact(ContactCheckDirection.Up))
            {
                Core.Velocity.y = 0;
            }
        }

        private enum ContactCheckDirection
        {
            Up,
            Down
        }

        private bool CheckBoundsContact(ContactCheckDirection direction)
        {
            var bounds = Core.GetBounds();
            var castOrigin = direction == ContactCheckDirection.Down
                ? new Vector2(bounds.min.x + bounds.extents.x, bounds.min.y)
                : new Vector2(bounds.max.x - bounds.extents.x, bounds.max.y);
            var castDirection = direction == ContactCheckDirection.Down ? Vector2.down : Vector2.up;

            var contactCount = Physics2D.BoxCastNonAlloc(castOrigin,
                new Vector2(bounds.size.x, contactRaycastSizeAndDistance),
                0, castDirection,
                _contacts, contactRaycastSizeAndDistance, contactCheckMask);
            return _contacts
                .Take(contactCount)
                .Where(contact => !Core.RaycastIgnoredColliders.Contains(contact.collider))
                .Where(contact =>
                    (contact.normal.y >= minContactAngle && direction == ContactCheckDirection.Down) ||
                    (contact.normal.y <= -minContactAngle && direction == ContactCheckDirection.Up))
                .Any(contact => Mathf.Abs(contact.point.y - castOrigin.y) < contactPositionEpsilon);
        }
    }
}