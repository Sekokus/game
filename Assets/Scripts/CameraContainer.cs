using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraContainer : MonoBehaviour
    {
        public Camera Camera { get; private set; }
        public CameraEffects Effects { get; private set; }

        private void Awake()
        {
            Camera = Camera.main;
            if (!Camera)
            {
                Debug.LogError("Camera is null");
                return;
            }

            Effects = Camera.GetComponent<CameraEffects>();
        }
    }
}