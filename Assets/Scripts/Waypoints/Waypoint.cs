using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
  public bool isTarget;

  void Start ()
  {
  }

  void Update ()
  {

  }

  void OnEnable()
  {
    isTarget = false;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "Player" && isTarget)
    {
      WaypointSpawner.Instance.RemoveWaypoint(gameObject);
    }
  }
}

