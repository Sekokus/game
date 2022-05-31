using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [DefaultExecutionOrder(-2)]
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private new Transform transform;
        [SerializeField] private Animator animator;
        public Animator Animator => animator;
        public Transform Transform => transform;
        public Rigidbody2D Rigidbody => rigidbody2D;
        public MovementModule Movement { get; private set; }
        public JumpModule Jump { get; private set; }
        public DashModule Dash { get; private set; }
        public InputModule Input { get; private set; }
        public AttackModule Attack { get; private set; }
        public ResourcesModule Resources { get; private set; }
        public GameEvents GameEvents { get; private set; }
        public PlayerAnimationEvents AnimationEvents { get; private set; }

        [NonSerialized] public Vector2 Velocity;

        private void OnValidate()
        {
            transform = base.transform;
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            GameEvents = Container.Get<GameEvents>();
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