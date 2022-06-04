using System;
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

    private void LookOn(Vector2 lookOn)
    {
        var direction = lookOn - (Vector2)transform.position;
        RotateInDirection(direction);
    }

    private void RotateInDirection(Vector2 direction)
    {
        var z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var newRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, z),
            aggroRotationSpeed * Time.deltaTime);
        transform.rotation = newRotation;
    }

    private float _currentAutoRotationSpeed;
    private bool _shouldRotateFromWall;

    private void Update()
    {
        if (_rotateToPlayer && _player != null)
        {
            rotation.enabled = false;
            Vector2 rotationTarget = _player.Transform.position;
            LookOn(rotationTarget);
        }
        else
        {
            rotation.enabled = true;
            UpdateAutoRotationSpeed();
        }
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

    public void SwitchEnable()
    {
        _rotateToPlayer = true;
    }

    public void SwitchDisable()
    {
        _rotateToPlayer = false;
    }
}