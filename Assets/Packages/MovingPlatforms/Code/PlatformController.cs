using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Bundos.MovingPlatforms
{
    public enum WaypointPathType
    {
        Closed,
        Open
    }

    public enum WaypointBehaviorType
    {
        Loop,
        PingPong
    }

    public class PlatformController : MonoBehaviour
    {
        [HideInInspector]
        public List<Vector3> waypoints = new List<Vector3>();
    
        [Header("Editor Settings")]
        public float handleRadius = .5f;
        public Vector2 snappingSettings = new Vector2(.1f, .1f);
        public Color gizmoDeselectedColor = Color.blue;

        [Header("Platform Waypoint Settings")]
        [SerializeField] private Rigidbody2D rb;
        public bool editing = false;

        public WaypointPathType pathType = WaypointPathType.Closed;
        public WaypointBehaviorType behaviorType = WaypointBehaviorType.Loop;

        public float moveSpeed = 5f; // Speed of movement
        public float stopDistance = 0.1f; // Distance to consider reaching a waypoint

        private int lastWaypointIndex = -1;
        private int currentWaypointIndex = 0;
        private int direction = 1; // 1 for forward, -1 for reverse
        public int RemainingCount = 0;
        public void SetRemainingWaypointsCount(int count)//设置剩余前进路径点数量
        {
            RemainingCount = count;
        }
        private void Update()
        {
            if (RemainingCount == 0)
            {
                return;
            }
            if (waypoints.Count == 0)
                return;

            if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex]) <= stopDistance)
            {
                RemainingCount--;
                if (pathType == WaypointPathType.Closed)
                {
                    switch (behaviorType)
                    {
                        case WaypointBehaviorType.Loop:
                            lastWaypointIndex = currentWaypointIndex;
                            currentWaypointIndex = mod((currentWaypointIndex + direction), waypoints.Count);
                            break;
                        case WaypointBehaviorType.PingPong:
                            if ((lastWaypointIndex == 1 && currentWaypointIndex == 0 && direction < 0) || (lastWaypointIndex == waypoints.Count - 1 && currentWaypointIndex == 0 && direction > 0))
                            {
                                direction *= -1;
                            }

                            lastWaypointIndex = currentWaypointIndex;
                            currentWaypointIndex = mod((currentWaypointIndex + direction), waypoints.Count);
                            break;
                    }
                }
                else if (pathType == WaypointPathType.Open)
                {
                    switch (behaviorType)
                    {
                        case WaypointBehaviorType.Loop:
                            int nextWaypointIndex = mod((currentWaypointIndex + direction), waypoints.Count);

                            if ((lastWaypointIndex == 1 && currentWaypointIndex == 0 && direction < 0) || (lastWaypointIndex == waypoints.Count - 2 && currentWaypointIndex == waypoints.Count - 1 && direction > 0))
                            {
                                transform.position = waypoints[nextWaypointIndex];
                            }

                            lastWaypointIndex = currentWaypointIndex;
                            currentWaypointIndex = mod((currentWaypointIndex + direction), waypoints.Count);
                            break;
                        case WaypointBehaviorType.PingPong:
                            if ((lastWaypointIndex == 1 && currentWaypointIndex == 0 && direction < 0) || (lastWaypointIndex == waypoints.Count - 2 && currentWaypointIndex == waypoints.Count - 1 && direction > 0))
                            {
                                direction *= -1;
                            }

                            lastWaypointIndex = currentWaypointIndex;
                            currentWaypointIndex = mod((currentWaypointIndex + direction), waypoints.Count);
                            break;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            MoveToWaypoint(waypoints[currentWaypointIndex]);
        }
        private void MoveToWaypoint(Vector3 waypoint)
        {
            Vector2 direction = (waypoint - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        private void OnDrawGizmos()
        {
            if (IsSelected() && editing)
                return;

            if (pathType == WaypointPathType.Closed)
            {
                for (int i = 0; i < waypoints.Count; i++)
                {
                    Gizmos.color = gizmoDeselectedColor;

                    Vector3 nextPoint = waypoints[(i + 1) % waypoints.Count];
                    Gizmos.DrawLine(waypoints[i], nextPoint);

                    Gizmos.DrawSphere(waypoints[i], handleRadius / 2);
                }
            }
            else
            {
                for (int i = 0; i < waypoints.Count; i++)
                {
                    Gizmos.color = gizmoDeselectedColor;

                    Vector3 nextPoint = waypoints[(i + 1) % waypoints.Count];
                    if (i != waypoints.Count - 1)
                        Gizmos.DrawLine(waypoints[i], nextPoint);

                    Gizmos.DrawSphere(waypoints[i], handleRadius / 2);
                }
            }
        }

        private bool IsSelected()
        {
            return UnityEditor.Selection.activeGameObject == transform.gameObject;
        }

        int mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
