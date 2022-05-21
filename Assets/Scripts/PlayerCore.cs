using System;
using UnityEngine;

namespace Sekokus
{
    public class PlayerCore : MonoBehaviour
    {
        public BoxCollider2D Collider { get; private set; }
        public Animator Animator { get; private set; }
        public Transform Transform { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public MovementModule Movement { get; private set; }
        public JumpModule Jump { get; private set; }
        public DashModule Dash { get; private set; }
        public InputModule Input { get; private set; }
        public ResourcesModule Resources { get; private set; }

        [NonSerialized] public Vector2 Velocity;

        private void Awake()
        {
            Transform = transform;
            Rigidbody = GetComponent<Rigidbody2D>();
            Movement = GetComponent<MovementModule>();
            Jump = GetComponent<JumpModule>();
            Dash = GetComponent<DashModule>();
            Collider = GetComponent<BoxCollider2D>();
            Animator = GetComponent<Animator>();
            Input = GetComponent<InputModule>();
            Resources = GetComponent<ResourcesModule>();
        }
    }
}