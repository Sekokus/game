using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MultiImageButtonGraphics : MonoBehaviour
    {
        [SerializeField] private Graphic[] graphics;

        public IEnumerable<Graphic> Graphics => graphics;
        private void Reset()
        {
            graphics = GetComponentsInChildren<Graphic>();
        }
    }
}