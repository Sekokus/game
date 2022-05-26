using UnityEngine;
using Utilities;

namespace Enemies
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletType bulletType;
        [SerializeField] [Min(0)] private float lifeTime = 5;
        [SerializeField] private Hitbox hitbox;
        public BulletType BulletType => bulletType;

        private PauseService _pauseService;
        private PauseObserver _pauseObserver;
        private BulletPool _bulletPool;
        private Timer _destroyTimer;

        private float _moveSpeed;

        private void Awake()
        {
            _destroyTimer = new Timer();
            _destroyTimer.Timeout += Destroy;

            hitbox.HitDamageable += hurtbox =>
            {
                hurtbox.ReceiveHit(hitbox);
                Destroy();
            };
            hitbox.HitNonDamageable += _ => Destroy();

            Construct();
        }

        private void Construct()
        {
            var appEvents = Container.Get<ApplicationEvents>();
            appEvents.Exiting += () => Destroy(gameObject);

            _bulletPool = Container.Get<BulletPool>();
            _pauseService = Container.Get<PauseService>();
            _pauseObserver = _pauseService.GetObserver(PauseSource.Any);
        }

        public void Shot(Vector2 direction, float speed)
        {
            if (IsShot)
            {
                return;
            }

            _moveSpeed = speed;
            RotateTowardsShotDirection(direction);

            _destroyTimer.Start(lifeTime);
        }

        private void RotateTowardsShotDirection(Vector2 direction)
        {
            transform.right = direction;
        }

        private void FixedUpdate()
        {
            if (_pauseObserver.IsPaused || !IsShot)
            {
                return;
            }

            var deltaTime = Time.fixedDeltaTime;
            Move(deltaTime);
            _destroyTimer.Tick(deltaTime);
        }

        private void Move(float deltaTime)
        {
            var move = Vector3.right * (_moveSpeed * deltaTime);
            transform.Translate(move);
        }

        private bool IsShot => _moveSpeed > 0;

        private void Destroy()
        {
            _bulletPool.Add(this);
        }

        private void OnDisable()
        {
            _moveSpeed = 0;
        }
    }
}