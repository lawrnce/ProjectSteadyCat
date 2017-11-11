using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Movement/Data", order = 1)]
public class MovementData : ScriptableObject
{
  [Header("Movement")]
  public AnimationCurve radiusCurve;

  [Range(2.0f, 5.0f)]
  public float maxRadiusDivisor = 3.0f;

  [Range(5.0f, 15.0f)]
  public float minRadiusDivisor = 10.0f;

  [Space(20)]
  [Header("Waypoint")]
  public AnimationCurve driftCurve;
  public AnimationCurve distanceCurve;

  [Range(0.0f, 5.0f)]
  public float minDistance = 0.4f;

  [Range(5.0f, 25.0f)]
  public float maxDistance = 2.0f;
}

