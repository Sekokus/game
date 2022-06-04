using System;
using System.Collections;
using DefaultNamespace;
using Enemies;
using Player;
using UnityEngine;

public class TurretAim : MonoBehaviour, ISwitchable
{
    private PlayerCore _player;

    [SerializeField] private float aggroRotationSpeed = 5;
    [SerializeField] private float idleRotationSpeed = 3;

    [SerializeField] private float idleRotationDirectionChangeHitDistance = 1;
    [SerializeField] private float aimedMinDot = 0.8f;
    [SerializeField] private float aimTime = 2;
    [SerializeField] private float blinkTime = 0.1f;
    [SerializeField] private float aimedRotationSpeed = 2;

    [SerializeField] private TransformRotation rotation;
    [SerializeField] private LaserBeam laserBeam;

    private void Reset()
    {
        laserBeam = GetComponent<LaserBeam>();
        rotation = GetComponent<TransformRotation>();
    }

    private void OnValidate()
    {
        rotation.rotationSpeed = idleRotationSpeed;
    }

    private void Awake()
    {
        Container.Get<PlayerFactory>().WhenPlayerAvailable(playerCore => _player = playerCore);
    }

    private void RotateInDirection(Vector2 direction)
    {
        var speed = _isAimed ? aimedRotationSpeed : aggroRotationSpeed;
        var z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var newRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, z),
            speed * Time.deltaTime);
        transform.rotation = newRotation;
    }

    private float _currentAutoRotationSpeed;
    private bool _shouldRotateFromWall;

    private void Update()
    {
        if (_rotateToPlayer && _player != null)
        {
            rotation.enabled = false;
            RotateInDirection(GetDirectionToPlayer());

            if (!_isAimed && ShouldAim())
            {
                SetAimLock();
            }
        }
        else
        {
            rotation.enabled = true;
            UpdateAutoRotationSpeed();
        }
    }

    private void SetAimLock()
    {
        _isAimed = true;
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        // TODO:
        yield break;
    }

    private bool ShouldAim()
    {
        var directionToPlayer = GetDirectionToPlayer();
        var currentLookDirection = laserBeam.GetBeamDirection();
        var dot = Vector2.Dot(directionToPlayer, currentLookDirection);
        return dot >= aimedMinDot;
    }

    private Vector2 GetDirectionToPlayer()
    {
        return ((Vector2)_player.Transform.position - laserBeam.GetOriginPoint()).normalized;
    }

    private bool ShouldRotateFromWall()
    {
        var lastRaycast = laserBeam.LastRaycast;
        if (!lastRaycast)
        {
            return false;
        }

        return (lastRaycast.distance < idleRotationDirectionChangeHitDistance);
    }

    private void UpdateAutoRotationSpeed()
    {
        var shouldRotateFromWallNew = ShouldRotateFromWall();
        if (shouldRotateFromWallNew == _shouldRotateFromWall)
        {
            return;
        }

        _shouldRotateFromWall = shouldRotateFromWallNew;
        if (shouldRotateFromWallNew)
        {
            rotation.rotationSpeed *= -1;
        }
    }

    private bool _rotateToPlayer = false;
    private bool _isAimed;

    public void SwitchEnable()
    {
        _rotateToPlayer = true;
    }

    public void SwitchDisable()
    {
        _rotateToPlayer = false;
    }
}