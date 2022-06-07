using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
    [DefaultExecutionOrder(-2)]
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private new Transform transform;
        [SerializeField] private Animator animator;
        [SerializeField] private BoxCollider2D boxCollider;
        public Animator Animator => animator;
        public Transform Transform => transform;
        public Rigidbody2D Rigidbody => rigidbody2D;
        public MovementModule Movement { get; private set; }
        public JumpModule Jump { get; private set; }
        public DashModule Dash { get; private set; }
        public InputModule Input { get; private set; }
        public AttackModule Attack { get; private set; }
        public ResourcesModule Resources { get; private set; }
        public GameState GameState { get; private set; }
        public PlayerAnimationEvents AnimationEvents { get; private set; }
        public CameraContainer CameraContainer { get; private set; }

        public BoxCollider2D BoxCollider => boxCollider;

        public readonly List<Collider2D> RaycastIgnoredColliders = new List<Collider2D>();

        public Bounds GetBounds()
        {
            var rawBounds = BoxCollider.bounds;
            return new Bounds(rawBounds.center, rawBounds.size + Vector3.one * (BoxCollider.edgeRadius * 2));
        }

        public event Action DownAction;
        
        public void PostDownAction()
        {
            DownAction?.Invoke();
        }

        [NonSerialized] public Vector2 Velocity;

        private void Reset()
        {
            transform = base.transform;
            rigidbody2D = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Awake()
        {
            GameState = Container.Get<GameState>();
            CameraContainer = Container.Get<CameraContainer>();
            Movement = GetComponent<MovementModule>();
            Jump = GetComponent<JumpModule>();
            Dash = GetComponent<DashModule>();
            Input = GetComponent<InputModule>();
            Attack = GetComponent<AttackModule>();
            Resources = GetComponent<ResourcesModule>();
            AnimationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        }

        private readonly List<PlayerRestrictions> _restrictions = new List<PlayerRestrictions>();

        public void AddRestrictions(params PlayerRestrictions[] actions)
        {
            _restrictions.AddRange(actions);
        }

        public void RemoveRestrictions(params PlayerRestrictions[] actions)
        {
            foreach (var action in actions)
            {
                _restrictions.Remove(action);
            }
        }

        public bool CanPerform(PlayerRestrictions action)
        {
            return !_restrictions.Contains(action);
        }
    }
}