using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DashEffectController : MonoBehaviour
{
    [Header("Animation properties")]
    [SerializeField] private bool popAnimationEnabled = true;
    [SerializeField, Min(0)] private float startScale;
    [SerializeField, Min(0)] private float endScale;

    [Space]
    [Header("Trail properties")]
    [SerializeField] private bool trailEnabled = true;
    [SerializeField, Min(1)] private int maxLinePointsPerFrame = 4;
    
    [Space]
    [Header("Controller components")]
    [SerializeField] private SpriteRenderer animationRenderer;
    [SerializeField] private LineRenderer lineRenderer;

    private Coroutine _activeAnimationCoroutine;

    private Queue<Vector3> _linePositions;

    private void Awake()
    {
        animationRenderer.enabled = false;
        _linePositions = new Queue<Vector3>(maxLinePointsPerFrame);
    }

    private void InterruptActiveAnimation()
    {
        if (_activeAnimationCoroutine != null)
        {
            StopCoroutine(_activeAnimationCoroutine);
        }
    }

    public void OnDashStart(float animationTime)
    {
        InterruptActiveAnimation();
        _activeAnimationCoroutine = StartCoroutine(AnimationCoroutine(animationTime));
    }

    public void OnDashEnd(float animationTime)
    {
        lineRenderer.positionCount = 0;
        _linePositions.Clear();
        
        InterruptActiveAnimation();
        _activeAnimationCoroutine = StartCoroutine(AnimationCoroutine(animationTime, true));
    }

    public void OnDashFrameStart(Vector2 position)
    {
        if (!trailEnabled)
        {
            return;
        }

        if (lineRenderer.positionCount == maxLinePointsPerFrame)
        {
            _linePositions.Dequeue();
        }
        else
        {
            ++lineRenderer.positionCount;
        }

        _linePositions.Enqueue(position);
        lineRenderer.SetPositions(_linePositions.ToArray());
    }

    private IEnumerator AnimationCoroutine(float animationTime, bool backwards = false)
    {
        if (!popAnimationEnabled)
        {
            yield break;
        }

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

        lineRenderer.positionCount = 0;
        animationRenderer.enabled = false;
    }
}
