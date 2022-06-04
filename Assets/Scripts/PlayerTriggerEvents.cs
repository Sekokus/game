using System;
using Player;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerTriggerEvents : MonoBehaviour
    {
        public event Action<PlayerCore> Entered;
        public event Action<PlayerCore> Exited;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponentInParent<PlayerCore>();
            if (player)
            {
                Entered?.Invoke(player);
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            var player = col.GetComponentInParent<PlayerCore>();
            if (player)
            {
                Exited?.Invoke(player);
            }
        }
    }
}