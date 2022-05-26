using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [DefaultExecutionOrder(-2)]
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator Animator => animator;
        public Transform Transform { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public MovementModule Movement { get; private set; }
        public JumpModule Jump { get; private set; }
        public DashModule Dash { get; private set; }
        public InputModule Input { get; private set; }
        public AttackModule Attack { get; private set; }
        public ResourcesModule Resources { get; private set; }

        [NonSerialized] public Vector2 Velocity;

        private void Awake()
        {
            Transform = transform;
            Rigidbody = GetComponent<Rigidbody2D>();
            Movement = GetComponent<MovementModule>();
            Jump = GetComponent<JumpModule>();
            Dash = GetComponent<DashModule>();
            Input = GetComponent<InputModule>();
            Attack = GetComponent<AttackModule>();
            Resources = GetComponent<ResourcesModule>();
        }

        private readonly List<PlayerActionType> _restrictions = new List<PlayerActionType>();

        public void AddRestrictions(params PlayerActionType[] actions)
        {
            _restrictions.AddRange(actions);
        }

        public void RemoveRestrictions(params PlayerActionType[] actions)
        {
            foreach (var action in actions)
            {
                _restrictions.Remove(action);
            }
        }

        public bool CanPerform(PlayerActionType action)
        {
            return !_restrictions.Contains(action);
        }
    }
}