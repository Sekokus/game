using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class WaypointMovement : MonoBehaviour
    {
        private enum MoveMode
        {
            Loop,
            PingPong
        }

        [SerializeField] private MoveMode moveMode = MoveMode.Loop;
        [SerializeField, Min(0)] private float moveSpeed = 4;
        [SerializeField, Min(0)] private float waitTimeOnEachWaypoint = 2;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform target;

        private int _currentWaypoint;
        private int _incrementSign = 1;
        private bool _isWaiting;

        private void Update()
        {
            if (_isWaiting)
            {
                return;
            }

            MoveToWaypoint(_currentWaypoint);
            if (IsWaypointReached(_currentWaypoint))
            {
                OnWaypointReached();
            }
        }

        private void OnWaypointReached()
        {
            switch (moveMode)
            {
                case MoveMode.Loop:
                    _currentWaypoint = (_currentWaypoint + 1) % waypoints.Length;
                    break;
                case MoveMode.PingPong:
                    if (ShouldChangeIncrementSign())
                    {
                        _incrementSign *= -1;
                    }

                    _currentWaypoint += _incrementSign;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _isWaiting = true;
            Do.After(() => _isWaiting = false, waitTimeOnEachWaypoint)
                .Start(this);
        }

        private bool ShouldChangeIncrementSign()
        {
            return _currentWaypoint + 1 == waypoints.Length && _incrementSign == 1 ||
                   _currentWaypoint == 0 && _incrementSign == -1;
        }

        private bool IsWaypointReached(int currentWaypoint)
        {
            var waypoint = waypoints[currentWaypoint];
            return (target.position - waypoint.position).sqrMagnitude < 1e-5;
        }

        private void MoveToWaypoint(int currentWaypoint)
        {
            var waypoint = waypoints[currentWaypoint];
            target.position = Vector3.MoveTowards(transform.position, waypoint.position,
                moveSpeed * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Length < 2)
            {
                return;
            }

            GizmosHelper.PushColor(Color.blue);
            for (var i = 0; i < waypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }

            if (moveMode == MoveMode.Loop)
            {
                Gizmos.DrawLine(waypoints[0].position, waypoints[waypoints.Length - 1].position);
            }

            foreach (var waypoint in waypoints)
            {
                Gizmos.DrawSphere(waypoint.position, 0.2f);
            }

            GizmosHelper.PopColor();
        }
    }
}