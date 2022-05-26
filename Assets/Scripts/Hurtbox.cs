using System;
using System.Collections;
using UnityEngine;

namespace Sekokus
{
    public class Hurtbox : MonoBehaviour
    {
        [SerializeField] private float disableOnHitTime;
        private Collider2D _collider;

        public float DisableOnHitTime => disableOnHitTime;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public event Action<Hitbox> HitReceived;

        public bool ReceiveHit(Hitbox hitbox)
        {
            if (!_collider.enabled)
            {
                return false;
            }

            HitReceived?.Invoke(hitbox);
            StartCoroutine(DisableOnHitRoutine(disableOnHitTime));
            return true;
        }

        private IEnumerator DisableOnHitRoutine(float time)
        {
            _collider.enabled = false;

            yield return new WaitForSeconds(time);

            _collider.enabled = true;
        }
    }
}