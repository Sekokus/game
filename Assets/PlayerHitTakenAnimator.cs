using Sekokus;
using System.Collections;
using UnityEngine;

public class PlayerHitTakenAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float switchInterval;

    private float _animationTime;

    private void Awake()
    {
        var hurtbox = GetComponent<Hurtbox>();
        hurtbox.HitReceived += OnHitReceived;
        _animationTime = hurtbox.DisableOnHitTime;
    }

    private void OnHitReceived(Hitbox obj)
    {
        StartCoroutine(AnimationRoutine(_animationTime, switchInterval));
    }

    private IEnumerator AnimationRoutine(float animationTime, float interval)
    {
        var passedTime = 0f;
        var passedTimeSinceSwitch = 0f;
        while (passedTime < animationTime)
        {
            var deltaTime = Time.deltaTime;
            passedTime += deltaTime;
            passedTimeSinceSwitch += deltaTime;

            var doSwitch = passedTimeSinceSwitch >= interval;
            passedTimeSinceSwitch -= interval;

            if (doSwitch)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            yield return null;
        }

        spriteRenderer.enabled = true;
    }
}
