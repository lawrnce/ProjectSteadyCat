using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerBackground : MonoBehaviour
{
  public GameObject pixelSpritePrefab;
  public float maxScale;

  private float screenHeight;
  private float screenWidth;
  private float pixelSize;
  private float spriteWidth;
  private GameObject[] pixelSprites = new GameObject[8];

  void Start ()
  {
    SetupSpriteSize();
    SetupVisualizer();
  }

  void Update ()
  {
    UpdateVisualizer();
  }

  void SetupSpriteSize()
  {
    screenHeight = Camera.main.orthographicSize * 2.0f;
    screenWidth = screenHeight / Screen.height * Screen.width;
    pixelSize = pixelSpritePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    spriteWidth = screenWidth/8.0f / pixelSize;
  }

  void SetupVisualizer()
  {
    float y = -(screenHeight / 2.0f);
    float dx = screenWidth / 8.0f;
    float xStart = -(screenWidth / 2.0f) + dx / 2.0f;

    for (int i = 0; i < 8; i++)
    {
      GameObject instancePixel = (GameObject) Instantiate(pixelSpritePrefab);
      instancePixel.name = "Visual Pixel " + i;
      instancePixel.transform.parent = this.transform;
      instancePixel.transform.position = new Vector3(xStart + i*dx, y, -1);
      pixelSprites[i] = instancePixel;
    }
  }

  void UpdateVisualizer()
  {
    for (int i = 0; i < 8; i++)
    {
      if (pixelSprites != null)
      {
        float bandHeight = AudioPeer.audioBandBuffer[i] * screenHeight/2.0f / pixelSize;
        pixelSprites[i].transform.localScale = new Vector3(spriteWidth, bandHeight, 0);
      }
    }
  }
}

