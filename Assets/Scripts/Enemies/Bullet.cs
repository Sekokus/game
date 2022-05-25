using System;
using System.Collections;
using System.Linq;
using Sekokus.Utilities;
using UnityEngine;

namespace Sekokus.Enemies
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletType bulletType;
        [SerializeField, Min(0)] private float lifeTime = 5;
        [SerializeField] private Vector2 colliderSize = new Vector2(1, 1);
        [Space] [SerializeField] private LayerMask collideWith;
        [SerializeField] private int overlapBufferSize = 8;

        public event Action<Collider2D[]> Hit;
        public BulletType BulletType => bulletType;

        private Collider2D[] _overlapBuffer;
        private int _overlapCount;
        
        private PauseService _pauseService;
        private PauseObserver _pauseObserver;
        private BulletPool _bulletPool;
        private Timer _destroyTimer;

        private float _moveSpeed;

        private void Awake()
        {
            _overlapBuffer = new Collider2D[overlapBufferSize];
            _destroyTimer = new Timer();
            _destroyTimer.Timeout += Destroy;
            
            Construct();
        }

        private void Construct()
        {
            _bulletPool = Container.Get<BulletPool>();
            _pauseService = Container.Get<PauseService>();
            _pauseObserver = _pauseService.GetObserver(PauseSource.Any);
        }

        private bool TestForOverlap()
        {
            var angle = GetOrientationAngle();
            var size = GetScaledSize();
            _overlapCount = Physics2D.OverlapBoxNonAlloc(transform.position,
                colliderSize, angle, _overlapBuffer, collideWith);

            return _overlapCount > 0;
        }

        private Vector2 GetScaledSize()
        {
            var scale = transform.localScale;
            return new Vector2(colliderSize.x * scale.x, colliderSize.y * scale.y);
        }

        private float GetOrientationAngle()
        {
            return transform.eulerAngles.z;
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
            if (_pauseObserver.IsPaused)
            {
                return;
            }

            if (IsShot)
            {
                var deltaTime = Time.fixedDeltaTime;
                Move(deltaTime);
                _destroyTimer.Tick(deltaTime);
            }

            if (!TestForOverlap())
            {
                return;
            }

            var overlaps = _overlapBuffer.Take(_overlapCount).ToArray();
            Hit?.Invoke(overlaps);

            Destroy();
        }

        private void Move(float deltaTime)
        {
            var move = Vector3.right * (_moveSpeed * deltaTime);
            transform.Translate(move);
        }

        private bool IsShot => _moveSpeed > 0;

        private void OnDrawGizmos()
        {
            GizmosHelper.PushColor(Color.green);
            GizmosHelper.PushMatrix(transform.localToWorldMatrix);

            Gizmos.DrawWireCube(Vector3.zero, colliderSize);

            GizmosHelper.PopColor();
            GizmosHelper.PopMatrix();
        }

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