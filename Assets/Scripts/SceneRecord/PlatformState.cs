using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bundos.MovingPlatforms;
[System.Serializable]
public class PlatformState
{
    public List<Vector3> waypoints = new List<Vector3>(); // 平台的路径点
    public float handleRadius = .5f;
    public Vector2 snappingSettings = new Vector2(.1f, .1f);
    public Color gizmoDeselectedColor = Color.blue;
    public bool editing = false;
    public WaypointPathType pathType = WaypointPathType.Closed;
    public WaypointBehaviorType behaviorType = WaypointBehaviorType.Loop;
    public float moveSpeed = 5f; // Speed of movement
    public float stopDistance = 0.1f; // Distance to consider reaching a waypoint
    public int lastWaypointIndex = -1;
    public int currentWaypointIndex = 0;
    public int direction = 1; // 1 for forward, -1 for reverse
    public int RemainingCount = 0;
    public Vector3 position; // 平台的初始位置

}
