using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player properties")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float minVerticalVelocity = -20f;
    [SerializeField, Min(0)] private float speed = 4f;
    [SerializeField, Range(0, 1)] private float airAccelerationFactor = 0.6f;
    [SerializeField, Min(0)] private float jumpStartSpeed = 5;
    [SerializeField, Range(0, 1)] private float jumpAbortFactor = 0.6f;
    [SerializeField, Range(0, 1)] private float jumpBufferTime = 0.15f;
    [SerializeField] private bool isInitiallyFlipped;

    [Header("Dash properties")] 
    [SerializeField] private UnityEvent<float> onDashStart;
    [SerializeField] private UnityEvent<float> onDashEnd;
    [SerializeField, Min(0)] private float dashDistance = 3f;
    [SerializeField, Min(0)] private float dashStartDelay = 0.1f;
    [SerializeField, Min(0)] private float dashEndDelay = 0.1f;

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
    [SerializeField, Range(0, 1)] private float followSmoothness;

    [Space]
    [Header("Player components")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private bool _isWaitingForJump;
    private bool _shouldAbortJumpLater;
    private bool _isGrounded;
    private Vector2 _lastNonZeroInput;
    private float _moveInput;
    private Vector2 _velocity;
    private float _jumpWaitTimePassed;
    private bool _isDashing;

    private RaycastHit2D[] _contacts;

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
        if (context.action.IsPressed() && !_isDashing)
        {
            Dash(_lastNonZeroInput.normalized);
        }
    }

    private void Dash(Vector2 direction)
    {
        IEnumerator DashCoroutine()
        {
            _isDashing = true;
            _velocity.y = 0;
            playerRigidbody.velocity = Vector2.zero;
            var startPosition = playerRigidbody.position;
            var targetPosition = startPosition + direction * dashDistance;
            
            onDashStart?.Invoke(dashStartDelay);

            yield return new WaitForSeconds(dashStartDelay);
            playerRigidbody.MovePosition(targetPosition);

            yield return new WaitForFixedUpdate();

            onDashEnd?.Invoke(dashEndDelay);
            yield return new WaitForSeconds(dashEndDelay);
            _isDashing = false;
        }
        
        StartCoroutine(DashCoroutine());
    }

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        _contacts = new RaycastHit2D[contactsBufferSize];
    }

    private void FixedUpdate()
    {
        HandleBoundsContacts();
        ApplyMoveInput();
        ApplyGravity();
        HandleJumping();
        ApplyVelocityToRigidbody();
        FocusCamera();
    }

    private void ApplyMoveInput()
    {
        if (_isDashing)
        {
            return;
        }
        Walk(_moveInput);
    }

    private enum ContactCheckDirection
    {
        Up, Down
    }

    private bool IsBoundsContact(ContactCheckDirection direction)
    {
        var bounds = playerCollider.bounds;
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
            _isGrounded = IsBoundsContact(ContactCheckDirection.Down);
            if (_isGrounded)
            {
                _velocity.y = 0;
            }
        }

        if (_velocity.y < 0)
        {
            return;
        }

        if (IsBoundsContact(ContactCheckDirection.Up))
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
        Gizmos.DrawCube(new Vector3(playerRigidbody.position.x, playerCollider.bounds.min.y, 0), new Vector3(0.3f, 0.05f, 0));
    }

    private void ApplyGravity()
    {
        if (_isDashing)
        {
            return;
        }
        _velocity.y += gravity * Time.fixedDeltaTime;
    }

    private void FocusCamera()
    {
        var player = playerRigidbody.position;
        var cameraTransform = playerCamera.transform;
        var cameraPosition = cameraTransform.position;
        var target = new Vector3(player.x, player.y, cameraPosition.z);
        cameraTransform.position = Vector3.Lerp(cameraPosition, target, followSmoothness);
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
        if (!_isWaitingForJump)
        {
            return;
        }
        _jumpWaitTimePassed += Time.fixedDeltaTime;
        if (!_isGrounded || !(_jumpWaitTimePassed <= jumpBufferTime))
        {
            return;
        }

        Jump();
        if (_shouldAbortJumpLater)
        {
            AbortJump();
            _shouldAbortJumpLater = false;
        }
        _jumpWaitTimePassed = 0;
        _isWaitingForJump = false;
    }

    private void InitiateJump()
    {
        _jumpWaitTimePassed = 0;
        _shouldAbortJumpLater = false;
        _isWaitingForJump = true;
    }

    private void AbortJump()
    {
        if (_velocity.y > 0)
        {
            _velocity.y *= jumpAbortFactor;
            _jumpWaitTimePassed = 0;
            _isWaitingForJump = false;
        }
        else
        {
            _shouldAbortJumpLater = true;
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
        playerSpriteRenderer.flipX = (direction > 0) != isInitiallyFlipped;
    }
}
