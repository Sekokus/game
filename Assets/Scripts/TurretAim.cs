using DefaultNamespace;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TurretAim : MonoBehaviour, ISwitchable
{
    private PlayerCore _player;

    [SerializeField] private Light2D headLight;
    [SerializeField] private float enabledIntensity;
    [SerializeField] private float disabledIntensity;

    [SerializeField] private float aggroRotationSpeed = 5;
    [SerializeField] private float idleRotationSpeed = 3;

    [SerializeField] private float idleRotationDirectionChangeHitDistance = 1;
    [SerializeField] private float aimTime = 2;
    [SerializeField] private float blinkTime = 0.1f;
    [SerializeField] private float aimedRotationMinSpeed = 2;

    [SerializeField] private LinearRotation rotation;
    [SerializeField] private LaserBeam laserBeam;
    [SerializeField] private BulletShooter shooter;
    [SerializeField] private PlayerRaycaster aimRaycaster;

    private void Reset()
    {
        laserBeam = GetComponent<LaserBeam>();
        rotation = GetComponent<LinearRotation>();
    }

    private void OnValidate()
    {
        rotation.rotationSpeed = idleRotationSpeed;
    }

    private void Awake()
    {
        Container.Get<PlayerFactory>().WhenPlayerAvailable(playerCore => _player = playerCore);
        aimRaycaster.Hit += (hit, core) => OnPlayerHit();
        _currentRotationSpeed = aggroRotationSpeed;
    }

    private void OnPlayerHit()
    {
        if (!_isAimed)
        {
            SetAimLock();
        }
    }

    private void RotateInDirection(Vector2 direction)
    {
        var z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var newRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, z),
            _currentRotationSpeed * Time.deltaTime);
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
        }
        else
        {
            rotation.enabled = true;
            UpdateAutoRotationSpeed();
        }
    }

    private float _currentRotationSpeed;

    private void SetAimLock()
    {
        _isAimed = true;
        var strength = 1f;
        var sign = -1;
        var lastTime = 0f;
        Do.EveryFrameFor(state =>
            {
                var passedBlinkTime = aimTime * state.Fraction % blinkTime;
                if (passedBlinkTime < lastTime)
                {
                    sign *= -1;
                }

                lastTime = passedBlinkTime;

                strength += sign * state.DeltaTime / (blinkTime / 2);
                laserBeam.SetStrength(strength);
            }, aimTime)
            .Start(this);

        Do.EveryFrameFor(state =>
            {
                _currentRotationSpeed =
                    aggroRotationSpeed - (aggroRotationSpeed - aimedRotationMinSpeed) * state.Fraction;
            }, aimTime)
            .Action(OnShoot)
            .Start(this);
    }

    private void OnShoot()
    {
        laserBeam.SetStrength(1);
        shooter.Shoot(transform.right);
        _isAimed = false;
        _currentRotationSpeed = aggroRotationSpeed;
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
        headLight.intensity = enabledIntensity;
    }

    public void SwitchDisable()
    {
        _rotateToPlayer = false;
        headLight.intensity = disabledIntensity;
    }
}