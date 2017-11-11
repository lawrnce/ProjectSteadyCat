using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteDirection : MonoBehaviour
{
  enum Direction {Up, Down, Left, Right};

  public Animator animator;
  [Range(0.0f, 1.0f)]
  public float thresholdRatio;
  private Direction state;
  private float pi = Mathf.PI;
  private float twoPI = 2.0f*Mathf.PI;

  void Start()
  {
    animator = GetComponent<Animator>();
    state = Direction.Right;
  }

  public void SetSpriteDirection(float angle)
  {
    /* float threshold = (pi - pi*thresholdRatio) / 2.0f; */

    /* if ((twoPI-threshold <= angle && twoPI > angle) || (0.0f <= angle && threshold > angle )) */
    /*   SetSpriteAnimation(Direction.Left); */

    /* else if (threshold <= angle && pi-threshold > angle) */
    /*   SetSpriteAnimation(Direction.Down); */

    /* else if (pi-threshold <= angle && pi+threshold > angle) */
    /*   SetSpriteAnimation(Direction.Right); */

    if ((pi * 1.5f <= angle && twoPI > angle) || (0.0f <= angle && pi/2.0f > angle))
      SetSpriteAnimation(Direction.Left);
    else if (pi/2.0f <= angle && pi * 1.5f > angle)
      SetSpriteAnimation(Direction.Right);
  }

  private void SetSpriteAnimation(Direction direction)
  {
    if (state == direction)
      return;

    state = direction;
    float scale = transform.localScale.x;

    switch(state)
    {
      case Direction.Left:
        animator.SetInteger("Direction", 0);
        transform.localScale = new Vector3(scale, -scale, 1.0f);
        break;
      case Direction.Down:
        animator.SetInteger("Direction", 0);
        transform.localScale = new Vector3(scale, scale, 1.0f);
        break;
      case Direction.Right:
        animator.SetInteger("Direction", 0);
        transform.localScale = new Vector3(scale, scale, 1.0f);
        break;
      case Direction.Up:
        animator.SetInteger("Direction", 1);
        transform.localScale = new Vector3(scale, scale, 1.0f);
        break;
    }
  }
}

