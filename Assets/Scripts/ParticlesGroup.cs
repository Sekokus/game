using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ParticlesGroup : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleSystems;

        private void Reset()
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        [ContextMenu(nameof(PlayOnce))]
        public void PlayOnce()
        {
            foreach (var system in particleSystems)
            {
                system.Play();
            }
        }
    }
}