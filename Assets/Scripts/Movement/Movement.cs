using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  public static Movement Instance;
  public MovementData movementData;
  public RhythmManager rhythmManager;
  public float movementRadius;
  public Vector3 direction;
  public float speed;
  public float distance;

  // Probably use ScriptableObject
  public float playerCollisionRadius;
  public float waypointCollisionRadius;

  private GameObject currentWaypoint;
  private Vector3 currentStartPosition;
  private float currentDistanceToCenter;
  private float maxRadius;
  private float minRadius;
  private float initialRadius;
  private float targetRadius;

  void Awake()
  {
    Instance = this;
  }

  void OnEnable()
  {
  }

  void OnDisable()
  {
  }

  void Start ()
  {
    SetRadiusBound();
  }

  void Update ()
  {
    CalculateDirection();
    CalculateDistance();
    CalculateRadius();
  }

  void LateUpdate()
  {
    // START ******
    // FOR TESTING PARAMETERS ONLY
    float screenHeight = Camera.main.orthographicSize * 2.0f;
    float screenWidth = screenHeight / Screen.height * Screen.width;
    maxRadius = screenWidth / movementData.maxRadiusDivisor;
    minRadius = screenWidth / movementData.minRadiusDivisor;
    // END ******
  }

  private void SetRadiusBound()
  {
    float screenHeight = Camera.main.orthographicSize * 2.0f;
    float screenWidth = screenHeight / Screen.height * Screen.width;

    maxRadius = screenWidth / movementData.maxRadiusDivisor;
    minRadius = screenWidth / movementData.minRadiusDivisor;

    movementRadius = maxRadius;
    initialRadius = 0.0f;
    targetRadius = 0.0f;
  }

  private void SetRadiusValues()
  {
    float angleRatio = WaypointSpawner.Instance.GetAngleRatio();
    float radius = movementData.radiusCurve.Evaluate(angleRatio) * (maxRadius - minRadius) + minRadius;

    if (initialRadius == 0.0f && targetRadius == 0.0f)
    {
      initialRadius = maxRadius;
      targetRadius = radius;
    }

    if (targetRadius != radius)
    {
      initialRadius = movementRadius;
      targetRadius = radius;
    }
  }

  private void CalculateRadius()
  {
    float magnitude = Mathf.Abs(initialRadius - targetRadius);
    float traveledRadius = magnitude * distance;

    if (initialRadius > targetRadius)
      movementRadius = initialRadius - traveledRadius;
    else
      movementRadius = initialRadius + traveledRadius;
  }

  private void CalculateDirection()
  {
    GameObject waypoint = WaypointSpawner.Instance.GetWaypoint();

    if (currentWaypoint == null || currentWaypoint != waypoint)
    {
      currentWaypoint = waypoint;
      currentStartPosition = waypoint.transform.position;
      currentDistanceToCenter = currentStartPosition.magnitude - waypointCollisionRadius;
      direction = new Vector3(-currentStartPosition.x, -currentStartPosition.y, 0.0f);
      direction.Normalize();

      SetRadiusValues();
      CalculateSpeed();
    }
  }

  private void CalculateDistance()
  {
    float total = currentDistanceToCenter + (movementRadius - playerCollisionRadius);
    float traveled = Vector3.Distance(currentStartPosition, currentWaypoint.transform.position);

    distance = traveled / total;
    if (distance > 1.0f)
      distance = 1.0f;
  }

  private void CalculateSpeed()
  {
    float distance = currentDistanceToCenter + (movementRadius - playerCollisionRadius);
    float time = rhythmManager.GetTimeToNextOnset();

    if (time > 0.0f)
    {
      float rawSpeed = distance/time / 2.0f;

      if (rawSpeed > 13.0f)
      {
        speed = 13.0f;
      }
      else if (rawSpeed < 8.0f)
      {
        speed = 8.0f;
      }
      else
      {
        speed = rawSpeed;
      }
    }
  }

}

