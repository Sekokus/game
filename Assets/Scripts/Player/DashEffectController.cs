using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DashEffectController : MonoBehaviour
    {
        [Header("Animation properties")] [SerializeField]
        private bool popAnimationEnabled = true;

        [SerializeField] [Min(0)] private float startScale;
        [SerializeField] [Min(0)] private float endScale;

        [Space] [Header("Trail properties")] [SerializeField]
        private bool trailEnabled = true;

        [SerializeField] [Min(1)] private int maxLinePointsPerFrame = 4;

        [Space] [Header("Controller components")]
        [SerializeField] private LineRenderer lineRenderer;

        private Coroutine _activeAnimationCoroutine;

        private Queue<Vector3> _linePositions;

        private void Awake()
        {
            _linePositions = new Queue<Vector3>(maxLinePointsPerFrame);

            var dashModule = GetComponentInParent<DashModule>();
            dashModule.DashStarted += OnDashStarted;
            dashModule.DashFrameStart += OnDashFrameStart;
            dashModule.DashEnded += OnDashEnded;
        }

        private void InterruptActiveAnimation()
        {
            if (_activeAnimationCoroutine != null)
            {
                StopCoroutine(_activeAnimationCoroutine);
            }
        }

        public void OnDashStarted(float animationTime)
        {
            InterruptActiveAnimation();
            _activeAnimationCoroutine = StartCoroutine(AnimationCoroutine(animationTime));
        }

        public void OnDashEnded(float animationTime)
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
        }
    }
}