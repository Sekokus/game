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
        var sign = Mathf.Sign(targetScale - 1);
        Do.EveryFrameFor((deltaTime, passedTime) =>
            {
                var scale = 1 + sign * (1 - targetScale) * passedTime;
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
        var sign = Mathf.Sign(1 - targetScale);
        _routine = Do.EveryFrameFor((deltaTime, passedTime) =>
            {
                var scale = targetScale + sign * (1 - targetScale) * passedTime;
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