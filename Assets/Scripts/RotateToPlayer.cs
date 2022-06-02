using Enemies;
using Player;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour, ISwitchable
{
    private PlayerCore _player;
    [SerializeField] private float rotationSpeed = 5;

    private Vector2 _initialLookTarget;

    private void Awake()
    {
        _initialLookTarget = transform.position + transform.right;
        Container.Get<PlayerFactory>().WhenPlayerAvailable(playerCore => _player = playerCore);
    }

    private void Rotate(Vector2 lookOn)
    {
        var direction = lookOn - (Vector2)transform.position;
        var euler = transform.eulerAngles;
        euler.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, euler.z),
            rotationSpeed * Time.deltaTime);
    }

    private void Update()
    {
        var rotationTarget =
            _rotateToPlayer && _player != null ? (Vector2)_player.Transform.position : _initialLookTarget;
        Rotate(rotationTarget);
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