using System;
using System.Collections;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private float disableOnHitTime;
    [SerializeField] private Collider2D attachedCollider;
    [SerializeField] private Team team;
    [SerializeField] private bool restoreHealthOnHit = false;

    public Team Team => team;

    public float DisableOnHitTime => disableOnHitTime;

    private void Reset()
    {
        attachedCollider = GetComponent<Collider2D>();
    }

    public event Action<Hitbox> HitReceived;

    public bool ReceiveHit(Hitbox hitbox)
    {
        if (!IsEnabled)
        {
            return false;
        }

        HitReceived?.Invoke(hitbox);
        StartCoroutine(DisableOnHitRoutine(disableOnHitTime));
        return true;
    }

    private bool IsEnabled => attachedCollider.enabled;

    public bool RestoreHealthOnHit => restoreHealthOnHit;

    private IEnumerator DisableOnHitRoutine(float time)
    {
        attachedCollider.enabled = false;

        yield return new WaitForSeconds(time);

        attachedCollider.enabled = true;
    }
}