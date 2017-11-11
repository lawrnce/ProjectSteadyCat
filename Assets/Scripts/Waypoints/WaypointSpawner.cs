using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSpawner : MonoBehaviour
{
  public static WaypointSpawner Instance;
  public MovementData movementData;
  public int maxWaypoints;

  private List<GameObject> waypoints;
  private float lastAngle;

  void Awake()
  {
    Instance = this;
  }

  void Start ()
  {
    waypoints = new List<GameObject>();
    CreateWaypoints();
  }

  void Update ()
  {
    CreateWaypoints();
  }

  void LateUpdate()
  {
    TranslateWaypoints();
  }

  public GameObject GetWaypoint()
  {
    SetTargetIfNeeded();
    return waypoints[0];
  }

  public float GetAngleRatio()
  {
    Vector3 currentWaypointPosition = waypoints[0].transform.position;
    Vector3 nextWaypointPosition = waypoints[1].transform.position;

    currentWaypointPosition.Normalize();
    nextWaypointPosition.Normalize();

    return Vector3.Angle(currentWaypointPosition, nextWaypointPosition) / 180.0f;
  }

  public void RemoveWaypoint(GameObject waypoint)
  {
    waypoints.Remove(waypoint);
    waypoint.SetActive(false);
  }

  private void SetTargetIfNeeded()
  {
    Waypoint waypoint = waypoints[0].GetComponent<Waypoint>();

    if (waypoint.isTarget == false)
      waypoint.isTarget = true;
  }

  private void TranslateWaypoints()
  {
    Vector2 direction = Movement.Instance.direction;
    float speed = Movement.Instance.speed;

    foreach (GameObject waypoint in waypoints)
    {
      waypoint.transform.Translate(direction * speed * Time.deltaTime);
    }
  }

  private void UpdateWaypointsIfNeeded()
  {
    GameObject waypoint = waypoints[0];
    Vector3 waypointPosition = waypoint.transform.position;

    if (waypointPosition.x < 0.01f && waypointPosition.y < 0.01f)
    {
      waypoints.RemoveAt(0);
      waypoint.SetActive(false);
    }
  }

  private void CreateWaypoints()
  {
    while (waypoints.Count != maxWaypoints)
    {
      if (waypoints.Count == 0)
      {
        GameObject waypoint = ObjectPooler.Instance.GetPooledObject("Waypoint");

        if (waypoint != null)
        {
          Vector3 firstPosition = new Vector3(2.0f, 0.0f, 0.0f);
          lastAngle = 0.0f;
          waypoint.transform.position = firstPosition;
          waypoint.SetActive(true);
          waypoints.Add(waypoint);
        }
      }

      SetNextWaypoint();
    }
  }

  // Returns angle in radians.
  private float NextAngle()
  {
    int sign = (Random.Range(0, 2) == 0) ? 1 : -1;
    float random = movementData.driftCurve.Evaluate(Random.value);
    float drift = random * Mathf.PI * sign;
    float degrees = drift + lastAngle;

    return degrees;
  }

  private float NextDistance()
  {
    float random = movementData.distanceCurve.Evaluate(Random.value);
    float max = movementData.maxDistance;
    float min = movementData.minDistance;

    return random * (max - min) + min;
  }

  private void SetNextWaypoint()
  {
    float angle = NextAngle();
    float distance = NextDistance();
    float x = distance * Mathf.Cos(angle);
    float y = distance * Mathf.Sin(angle);
    Vector3 lastPosition = waypoints[waypoints.Count - 1].transform.position;
    Vector3 nextPosition = new Vector3(lastPosition.x + x, lastPosition.y + y, 0.0f);

    GameObject waypoint = ObjectPooler.Instance.GetPooledObject("Waypoint");
    if (waypoint != null)
    {
      waypoint.SetActive(true);
      waypoint.transform.position = nextPosition;
      waypoints.Add(waypoint);
      lastAngle = angle;
    }
  }
}

