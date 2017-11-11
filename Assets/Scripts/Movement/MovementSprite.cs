using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSprite : MonoBehaviour
{
  public Sprite[] sprites;
  public float scale = 3.0f;
  public float speedScale;
  public GameObject movementSpriteObject;

  private List<GameObject> movingSprites;
  private float screenHeight;
  private float screenWidth;
  private Vector2 size;

  void Start ()
  {
    transform.hierarchyCapacity = 4;
    movingSprites = new List<GameObject>();

    CalculateSizes();
    CreateMovementBackground();
  }

  void LateUpdate ()
  {
    TranslateSprites();
  }

  // MARK: - Create methods
  private void CalculateSizes()
  {
    screenHeight = Camera.main.orthographicSize * 2.0f;
    screenWidth = screenHeight / Screen.height * Screen.width;
    size = new Vector2(screenWidth/scale, screenHeight/scale);
  }

  private void CreateMovementBackground()
  {
    for (int i = 0; i <= 3; i++)
    {
      GameObject sprite = GameObject.Instantiate(movementSpriteObject, transform);
      sprite.transform.position = GetPositionForIndex(i);
      sprite.transform.localScale = new Vector3(scale, scale, 1);

      SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();
      spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
      spriteRenderer.size = size;

      movingSprites.Add(sprite);
    }
  }

  private Vector3 GetPositionForIndex(int index)
  {
    int col = index % 2;
    int row = index / 2;

    float x = ((col == 0) ? -1.0f : 1.0f) * 0.5f * screenWidth;
    float y = ((row == 0) ? -1.0f : 1.0f) * 0.5f * screenHeight;

    return new Vector3(x, y, 0);
  }

  // MARK: - Update methods.
  private void TranslateSprites()
  {
    Vector2 direction = Movement.Instance.direction;
    float globalSpeed = Movement.Instance.speed;
    float speed = globalSpeed * speedScale;

    foreach (GameObject s in movingSprites)
    {
      s.transform.Translate(direction * speed * Time.deltaTime);
      RearrangeSpriteIfNeeded(s);
    }
  }

  private void RearrangeSpriteIfNeeded(GameObject sprite)
  {
    float widthThreshold = screenWidth;
    float heightThreshold = screenHeight;
    float widthOffset = 2.0f * screenWidth;
    float heightOffset = 2.0f * screenHeight;

    Vector3 rearrangePosition = Vector3.zero;
    Vector3 position = sprite.transform.position;

    if (position.x >= widthThreshold)
      rearrangePosition = new Vector3(-widthOffset, 0.0f, 0.0f);
    if (position.x <= -widthThreshold)
      rearrangePosition = new Vector3(widthOffset, 0.0f, 0.0f);

    if (position.y >= heightThreshold)
      rearrangePosition = new Vector3(0.0f, -heightOffset, 0.0f);
    if (position.y <= -heightThreshold)
      rearrangePosition = new Vector3(0.0f, heightOffset, 0.0f);

    // New sprite for randomness.
    if (rearrangePosition != Vector3.zero)
    {
      sprite.transform.Translate(rearrangePosition);
      SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();
      spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
  }
}
