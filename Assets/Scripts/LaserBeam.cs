using System;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteAlways]
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField] private float beamDistance = 3f;
        [SerializeField] private LayerMask contactWith = ~0;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GameObject hitCircleObject;

        public RaycastHit2D LastRaycast { get; private set; }


        private void Reset()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            var origin = GetOriginPoint();
            var direction = GetBeamDirection();
            LastRaycast = Physics2D.Raycast(origin, direction, beamDistance, contactWith);
            SetHitState(LastRaycast);
        }

        private void SetHitState(RaycastHit2D hit)
        {
            bool hasHit = hit;
            hitCircleObject.SetActive(hasHit);
            if (hasHit)
            {
                hitCircleObject.transform.position = hit.point;
            }
            var distance = hit ? hit.distance : beamDistance;
            SetBeamDistance(distance);
        }

        private void SetBeamDistance(float distance)
        {
            var origin = GetOriginPoint();
            var direction = GetBeamDirection();
            var beamTip = origin + direction * distance;
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, beamTip);
        }

        public Vector2 GetOriginPoint()
        {
            return transform.position;
        }

        public Vector2 GetBeamDirection()
        {
            return transform.right;
        }
    }
}