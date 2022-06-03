using System;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteInEditMode]
    public class SpriteOutline : MonoBehaviour
    {
        public Color color = Color.white;

        [Range(0, 16)] public int outlineSize = 1;

        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            SetOutline(true);
        }
        
        private void OnDisable()
        {
            SetOutline(false);
        }

        private void SetOutline(bool outline)
        {
            var mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", outline ? 1f : 0);
            mpb.SetColor("_OutlineColor", color);
            mpb.SetFloat("_OutlineSize", outlineSize);
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}