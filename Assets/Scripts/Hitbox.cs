using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sekokus
{
    public class Hitbox : MonoBehaviour
    {
        private readonly HashSet<Hurtbox> _hits = new HashSet<Hurtbox>();
        private Collider2D _collider;
        private Hurtbox _parent;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _parent = GetComponentInParent<Hurtbox>();
        }

        public void Enable()
        {
            _collider.enabled = true;
        }

        public void Disable()
        {
            _hits.Clear();
            _collider.enabled = false;
        }

        public event Action<Hurtbox> HitInflicted;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent<Hurtbox>(out var hurtbox))
            {
                return;
            }

            if (hurtbox == _parent || !_hits.Contains(hurtbox))
            {
                return;
            }

            var inflicted = hurtbox.ReceiveHit(this);
            if (!inflicted)
            {
                return;
            }

            _hits.Add(hurtbox);
            HitInflicted?.Invoke(hurtbox);
        }
    }
}