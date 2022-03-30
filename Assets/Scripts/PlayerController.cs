using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Velocity properties")]
    [SerializeField] private bool gravityEnabled = true;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float minVerticalVelocity = -20f;
    [SerializeField, Min(0)] private float speed = 4f;
    [SerializeField, Range(0, 1)] private float airAccelerationFactor = 0.6f;

    [Header("Jump properties")]
    [SerializeField] private bool jumpEnabled = true;
    [SerializeField, Min(0)] private float jumpStartSpeed = 5;
    [SerializeField, Range(0, 1)] private float jumpAbortFactor = 0.6f;
    [SerializeField, Range(0, 1)] private float jumpBufferTime = 0.15f;
    [SerializeField, Range(0, 0.3f)] private float coyoteTime = 0.08f;

    [Header("Dash properties")]
    [SerializeField] private bool dashEnabled = true;
    [SerializeField] private DashControlMode dashControlMode = DashControlMode.Mouse;
    [SerializeField] private UnityEvent<float> onDashStart;
    [SerializeField] private UnityEvent<float> onDashEnd;
    [SerializeField] private UnityEvent<Vector2> onDashFrameStart;
    [SerializeField, Min(0)] private float dashDistance = 3f;
    [SerializeField] private bool stopDashOnCollision = true;
    [SerializeField, Min(0)] private float dashStartDelay = 0.1f;
    [SerializeField, Min(0)] private float dashEndDelay = 0.1f;
    [SerializeField, Min(1)] private int dashFrames = 7;
    [SerializeField, Min(0)] private float dashReloadTime = 0.2f;

    [Space]
    [Header("Ground/Ceil/Wall checking properties")]
    [SerializeField] private LayerMask contactCheckMask;
    [SerializeField, Range(0, 1)] private float minContactAngle = 0.7f;
    [SerializeField, Range(0, 0.2f)] private float contactRaycastSizeAndDistance = 0.05f;
    [SerializeField, Range(0, 0.2f)] private float contactPositionEpsilon = 0.025f;
    [SerializeField, Min(1)] private int contactsBufferSize = 6;

    [Space]
    [Header("Camera settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CameraFollowMode cameraFollowMode;
    [SerializeField, Range(0, 1)] private float followSmoothness;

    [Space]
    [Header("Player components")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    [Space]
    [Header("Other")]
    [SerializeField] private bool isSpriteInitiallyFlipped;
    [SerializeField] private bool debugFeaturesEnabled = true;

    private bool _isGrounded;
    private float _moveInput;
    private Vector2 _velocity;
    private bool _isDashing;
    private RaycastHit2D[] _contacts;
    private Vector2 _awakePosition;
    private Vector2 _lastNonZeroInput;

    private readonly TimedTrigger _jumpWaitTrigger = new TimedTrigger();
    private readonly Trigger _jumpAbortTrigger = new Trigger();
    private readonly TimedTrigger _coyoteTimeTrigger = new TimedTrigger();
    private readonly TimedTrigger _dashReloadingTrigger = new TimedTrigger();

    private enum CameraFollowMode
    {
        Raw, Lerp, DoNotFollow
    }

    private enum ContactCheckDirection
    {
        Up, Down
    }

    private enum DashControlMode
    {
        Mouse, Keyboard
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        if (input.sqrMagnitude > 0)
        {
            _lastNonZeroInput = input;
        }
        _moveInput = input.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!jumpEnabled)
        {
            return;
        }
        if (context.action.IsPressed())
        {
            InitiateJump();
        }
        else
        {
            AbortJump();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!dashEnabled)
        {
            return;
        }

        if (!context.action.IsPressed() || _dashReloadingTrigger.IsSet)
        {
            return;
        }

        Vector2 dashDirection;
        switch (dashControlMode)
        {
            case DashControlMode.Mouse:
                var mousePosition = GetWorldMousePosition();
                dashDirection = (mousePosition - playerRigidbody.position).normalized;
                break;
            case DashControlMode.Keyboard:
                dashDirection = _lastNonZeroInput;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Dash(dashDirection);
    }

    private Vector2 GetWorldMousePosition()
    {
        var screenPosition = Mouse.current.position.ReadValue();
        return playerCamera.ScreenToWorldPoint(screenPosition);
    }

    private void Dash(Vector2 direction)
    {
        IEnumerator DashCoroutine()
        {
            _dashReloadingTrigger.Set();
            _isDashing = true;

            _velocity.y = 0;
            playerRigidbody.velocity = Vector2.zero;

            onDashStart?.Invoke(dashStartDelay);

            yield return new WaitForSeconds(dashStartDelay);

            for (int i = 1; i <= dashFrames; i++)
            {
                var startPosition = playerRigidbody.position;
                onDashFrameStart?.Invoke(startPosition);

                var distance = dashDistance / dashFrames;
                var expectedEndPosition = startPosition + direction * distance;
                if (stopDashOnCollision)
                {
                    var hit = Physics2D.BoxCast(startPosition, GetActualColliderBounds().size, 0, direction, distance, contactCheckMask);
                    if (hit)
                    {
                        expectedEndPosition = hit.centroid;
                    }
                }

                playerRigidbody.MovePosition(expectedEndPosition);
                yield return new WaitForFixedUpdate();

                const float maxDistanceError = 0.1f;
                if (stopDashOnCollision && Vector2.Distance(startPosition, playerRigidbody.position) < distance - maxDistanceError)
                {
                    break;
                }
            }

            onDashEnd?.Invoke(dashEndDelay);
            yield return new WaitForSeconds(dashEndDelay);

            _isDashing = false;
            _dashReloadingTrigger.ResetIn(dashReloadTime);
        }

        StartCoroutine(DashCoroutine());
    }

    private Bounds GetActualColliderBounds()
    {
        var rawBounds = playerCollider.bounds;
        if (playerCollider is BoxCollider2D boxCollider)
        {
            return new Bounds(rawBounds.center, rawBounds.size + Vector3.one * (boxCollider.edgeRadius * 2));
        }

        return rawBounds;
    }

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        _awakePosition = playerRigidbody.position;
        _contacts = new RaycastHit2D[contactsBufferSize];
    }

    private void FixedUpdate()
    {
        HandledDebugging();

        HandleBoundsContacts();
        ApplyMoveInput();
        ApplyGravity();
        HandleJumping();
        ApplyVelocityToRigidbody();

        if (cameraFollowMode == CameraFollowMode.Lerp)
        {
            MoveCamera();
        }

        _jumpWaitTrigger.Step(Time.fixedDeltaTime);
        _coyoteTimeTrigger.Step(Time.fixedDeltaTime);
        _dashReloadingTrigger.Step(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        if (cameraFollowMode != CameraFollowMode.Lerp)
        {
            MoveCamera();
        }
    }

    private void HandledDebugging()
    {
        if (!debugFeaturesEnabled)
        {
            return;
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            playerRigidbody.position = _awakePosition;
        }

#if UNITY_EDITOR
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }

    private void ApplyMoveInput()
    {
        if (_isDashing)
        {
            return;
        }
        Walk(_moveInput);
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

    private void HandleBoundsContacts()
    {
        if (_velocity.y < 0)
        {
            var hasContact = CheckBoundsContact(ContactCheckDirection.Down);
            if (_isGrounded && !hasContact)
            {
                _coyoteTimeTrigger.SetFor(coyoteTime);
            }

            _isGrounded = hasContact;
            if (_isGrounded)
            {
                _velocity.y = 0;
            }
        }

        if (_velocity.y < 0)
        {
            return;
        }

        if (CheckBoundsContact(ContactCheckDirection.Up))
        {
            _velocity.y = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCollider == null)
        {
            return;
        }
        Gizmos.color = _isGrounded ? Color.red : Color.green;
        Gizmos.DrawCube(new Vector3(playerRigidbody.position.x, GetActualColliderBounds().min.y, 0), new Vector3(0.3f, 0.05f, 0));
    }

    private void ApplyGravity()
    {
        if (!gravityEnabled)
        {
            return;
        }
        if (_isDashing)
        {
            return;
        }
        _velocity.y += gravity * Time.fixedDeltaTime;
    }

    private void MoveCamera()
    {
        var player = playerRigidbody.position;
        var cameraTransform = playerCamera.transform;
        var cameraPosition = cameraTransform.position;
        var target = new Vector3(player.x, player.y, cameraPosition.z);

        cameraTransform.position = cameraFollowMode switch
        {
            CameraFollowMode.Raw => target,
            CameraFollowMode.Lerp => Vector3.Lerp(cameraPosition, target, followSmoothness),
            CameraFollowMode.DoNotFollow => cameraPosition,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void ApplyVelocityToRigidbody()
    {
        if (_isDashing)
        {
            return;
        }

        if (_velocity.y < minVerticalVelocity)
        {
            _velocity.y = minVerticalVelocity;
        }
        playerRigidbody.velocity = _velocity;
    }

    private void HandleJumping()
    {
        if (_isDashing)
        {
            return;
        }

        if (_jumpWaitTrigger.IsFree)
        {
            return;
        }

        if (!_isGrounded && _coyoteTimeTrigger.IsFree)
        {
            return;
        }

        _jumpWaitTrigger.Reset();
        _coyoteTimeTrigger.Reset();

        Jump();
        if (_jumpAbortTrigger.CheckAndReset())
        {
            AbortJump();
        }
    }

    private void InitiateJump()
    {
        _jumpWaitTrigger.SetFor(jumpBufferTime);
        _jumpAbortTrigger.Reset();
    }

    private void AbortJump()
    {
        if (_velocity.y > 0)
        {
            _velocity.y *= jumpAbortFactor;
            _jumpWaitTrigger.Reset();
        }
        else
        {
            _jumpAbortTrigger.Set();
        }
    }

    private void Jump()
    {
        _velocity.y = jumpStartSpeed;
        _isGrounded = false;
    }

    private void Walk(float direction)
    {
        _velocity.x = speed * direction;
        if (!_isGrounded)
        {
            _velocity.x *= airAccelerationFactor;
        }

        if (Mathf.Abs(direction) > 0)
        {
            LookTo(direction);
        }
    }

    private void LookTo(float direction)
    {
        playerSpriteRenderer.flipX = (direction > 0) != isSpriteInitiallyFlipped;
    }
}
