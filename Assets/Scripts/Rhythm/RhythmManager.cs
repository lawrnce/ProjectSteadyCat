using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
  public RhythmTool rhythmTool;
  public AudioClip audioClip;

  private AnalysisData low;

  void Start ()
  {
    rhythmTool.NewSong(audioClip);
    low = rhythmTool.low;
    rhythmTool.SongLoaded += OnSongLoaded;
  }

  void OnSongLoaded()
  {
    rhythmTool.Play();
  }

  public float GetTimeToNextOnset()
  {
    for (int i=0; i < 1000; i++)
    {
      int frameIndex = Mathf.Min(rhythmTool.currentFrame + i, rhythmTool.totalFrames);

      if (IsValidOnsetForFrame(frameIndex))
      {
        return rhythmTool.frameLength * i;
      }
    }

    return 0.3f;
  }

  private bool IsValidOnsetForFrame(int frameIndex)
  {
    bool lowOnset = (low.GetOnset(frameIndex) > 0);
    bool beatOnset = (rhythmTool.IsBeat(frameIndex) == 1);

    return lowOnset || beatOnset;
  }
}
