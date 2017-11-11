using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadialMovement : MonoBehaviour
{
  private PlayerSpriteDirection spriteDirection;
  private float initialAngle;
  private float targetAngle;
  private float totalDistance;
  private float sign;
  private float twoPI = 2.0f*Mathf.PI;

  void Start ()
  {
    spriteDirection = GetComponent<PlayerSpriteDirection>();
    float radius = Movement.Instance.movementRadius;
    transform.position = new Vector3(-radius, 0.0f, 0.0f);
  }

  void Update ()
  {
  }

  void LateUpdate()
  {
    float angle = CalculateAngle(Movement.Instance.direction);
    if (angle != targetAngle)
    {
      initialAngle = CalculateAngle(transform.position);
      targetAngle = angle;
      CalculateRotation(initialAngle, targetAngle);
    }

    float percentage = Movement.Instance.distance;
    if (percentage > 0.97f)
      percentage = 1.0f;

    float coveredDistance = totalDistance * percentage * sign;
    float currentAngle = initialAngle + coveredDistance;

    SetAngle(currentAngle);
    RotateSprite(currentAngle);
    spriteDirection.SetSpriteDirection(currentAngle);
  }

  // Radial movement
  private float CalculateAngle(Vector3 vector)
  {
    float angle = Quaternion.FromToRotation(Vector3.right, vector).eulerAngles.z;
    if (angle == 0.0f && Vector3.Dot(Vector3.right, vector) < 0.0f)
      angle = 180.0f;

    return angle * Mathf.Deg2Rad;
  }

  private void SetAngle(float angle)
  {
    float radius = Movement.Instance.movementRadius;
    float x = radius * Mathf.Cos(angle);
    float y = radius * Mathf.Sin(angle);
    transform.position = new Vector3(x, y, 0.0f);
  }

  private void CalculateRotation(float intialAngle, float targetAngle)
  {
    totalDistance = Mathf.Abs(initialAngle - targetAngle);
    if (totalDistance > Mathf.PI)
     totalDistance = twoPI - totalDistance;

    float upperRange = initialAngle + Mathf.PI;
    if (upperRange > twoPI)
      sign = (TestRange(targetAngle, initialAngle, twoPI) || TestRange(targetAngle, 0.0f, upperRange % twoPI)) ? 1.0f : -1.0f;
    else
      sign = (TestRange(targetAngle, initialAngle, upperRange)) ? 1.0f : -1.0f;
  }

  private bool TestRange (float numberToCheck, float bottom, float top)
  {
    return (numberToCheck >= bottom && numberToCheck <= top);
  }

  // Sprite rotation
  private void RotateSprite(float currentAngle)
  {
    float degrees = (currentAngle + Mathf.PI) / Mathf.Deg2Rad;
    transform.eulerAngles = new Vector3(0.0f, 0.0f, degrees);
  }
}

