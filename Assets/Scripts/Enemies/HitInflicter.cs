using System;
using UnityEngine;

namespace Enemies
{
    public class HitInflicter : MonoBehaviour
    {
        [SerializeField] private Hitbox hitbox;
        
        private void Awake()
        {
            hitbox.HitDamageable += InflictHit;
        }

        private void InflictHit(Hurtbox obj)
        {
            obj.ReceiveHit(hitbox);
        }
    }
}