using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBlockButtonClickHandler : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float tweenTime = 0.1f;
    [SerializeField] private float targetScale = 0.85f;
    [SerializeField] private Transform scaleTarget;
    
    private Coroutine _routine;

    public event Action AnimationEnded;

    private void OnAnimationEnded()
    {
        AnimationEnded?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAnimation();
        Do.EveryFrameFor((deltaTime, fraction) =>
            {
                var scale = targetScale + (1 - targetScale) * fraction;
                scaleTarget.localScale = new Vector3(scale, scale, scale);
            }, tweenTime)
            .Start(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnAnimationEnded();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAnimation();
        _routine = Do.EveryFrameFor((deltaTime, fraction) =>
            {
                var scale = 1 + (targetScale - 1) * fraction;
                scaleTarget.localScale = new Vector3(scale, scale, scale);
            }, tweenTime)
            .Start(this);
    }

    private void StopAnimation()
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
        }
    }
}