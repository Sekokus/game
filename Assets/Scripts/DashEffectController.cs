using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DashEffectController : MonoBehaviour
{
    [Header("Animation properties")]
    [SerializeField, Min(0)] private float startScale;
    [SerializeField, Min(0)] private float endScale;

    [Space]
    [Header("Controller components")]
    [SerializeField] private SpriteRenderer animationRenderer;

    private Coroutine _activeCoroutine; 

    private void Awake()
    {
        animationRenderer.enabled = false;
    }

    private void InterruptActiveAnimation()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }
    }

    public void OnDashStart(float animationTime)
    {
        InterruptActiveAnimation();
        _activeCoroutine = StartCoroutine(AnimationCoroutine(animationTime));
    }

    public void OnDashEnd(float animationTime)
    {
        InterruptActiveAnimation();
        _activeCoroutine = StartCoroutine(AnimationCoroutine(animationTime, true));
    }

    private IEnumerator AnimationCoroutine(float animationTime, bool backwards = false)
    {
        animationRenderer.enabled = true;
        var scale = startScale;
        var time = 0f;
        while (Mathf.Abs(scale - endScale) > 0)
        {
            scale = Mathf.Lerp(startScale, endScale, time / animationTime);
            transform.localScale = Vector3.one * (backwards ? (endScale - scale + startScale) : scale);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        animationRenderer.enabled = false;
    }
}
