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

        private void SetOutlineInternal(bool outline)
        {
            var mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", outline ? 1f : 0);
            mpb.SetColor("_OutlineColor", color);
            mpb.SetFloat("_OutlineSize", outlineSize);
            spriteRenderer.SetPropertyBlock(mpb);
        }

        private void OnEnable()
        {
            SetOutline(_preDisableState);
        }

        private void OnDisable()
        {
            _preDisableState = outlineEnabled;
            SetOutline(false);
        }

        private void OnValidate()
        {
            SetOutlineInternal(outlineEnabled);
        }

        private bool _preDisableState = false;
        [SerializeField] private bool outlineEnabled;

        public void SetOutline(bool outline)
        {
            if (outlineEnabled == outline)
            {
                return;
            }
            
            SetOutlineInternal(outline);
            outlineEnabled = outline;
        }
    }
}