using Player;
using UnityEngine;
using Utilities;

namespace DefaultNamespace
{
    public class TwoWayPlatform : MonoBehaviour
    {
        [SerializeField] private bool isAlwaysPassableForPlayer;
        [SerializeField] private Collider2D platformCollider;

        private PlayerCore _player;
        private readonly Timer _passThroughTimer = new Timer();

        private void Reset()
        {
            platformCollider = GetComponent<Collider2D>();
        }

        private void Awake()
        {
            _passThroughTimer.Timeout += () => _suspendUpdate = false;

            var playerFactory = Container.Get<PlayerFactory>();
            playerFactory.WhenPlayerAvailable(player =>
            {
                _player = player;

                if (isAlwaysPassableForPlayer)
                {
                    _suspendUpdate = true;
                    IgnoreCollision();
                    return;
                }

                _player.DownAction += OnDownAction;
                _player.Dash.DashStarted += p =>
                {
                    _suspendUpdate = true;
                    IgnoreCollision();
                };
                _player.Dash.DashEnded += p =>
                {
                    _suspendUpdate = false;
                    IgnoreCollision();
                };
            });
        }

        private void OnDownAction()
        {
            IgnoreCollision();
            _suspendUpdate = true;
            _passThroughTimer.Start(passThroughTime);
        }

        private bool _collisionIgnored;
        [SerializeField] private float sunckPadding = 0.05f;
        [SerializeField, Range(0, 2)] private float passThroughTime = 0.1f;

        private void Update()
        {
            _passThroughTimer?.Tick(Time.deltaTime);
            if (_suspendUpdate || !_player)
            {
                return;
            }

            var canPassThrough = IsPlayerUnderPlatform();
            if (canPassThrough)
            {
                IgnoreCollision();
            }
            else
            {
                EnableCollision();
            }
        }

        private bool IsPlayerUnderPlatform()
        {
            var playerBottom = _player.GetBounds().min.y;
            var platformTop = platformCollider.bounds.max.y;

            return playerBottom < platformTop - sunckPadding;
        }

        private bool _collisionIgnoredForcefully;
        private bool _suspendUpdate;

        private void IgnoreCollision(bool force = false)
        {
            if (!force && _collisionIgnoredForcefully)
            {
                return;
            }

            _collisionIgnoredForcefully = force;
            SetIgnoreState(_player.BoxCollider, true);
        }

        private void EnableCollision() => SetIgnoreState(_player.BoxCollider, false);

        private void SetIgnoreState(Collider2D playerCollider, bool ignored)
        {
            if (_collisionIgnored == ignored)
            {
                return;
            }

            if (ignored)
            {
                _player.RaycastIgnoredColliders.Add(platformCollider);
            }
            else
            {
                _player.RaycastIgnoredColliders.Remove(platformCollider);
            }

            Physics2D.IgnoreCollision(playerCollider, platformCollider, ignored);
            _collisionIgnored = ignored;
        }
    }
}