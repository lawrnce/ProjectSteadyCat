using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEventManager : MonoBehaviour
{
  private Dictionary<string, Action<float>> eventDictionary;
  private static MovementEventManager eventManager;
  public static MovementEventManager instance
  {
    get
    {
      if (!eventManager)
      {
        eventManager = FindObjectOfType(typeof(MovementEventManager)) as MovementEventManager;

        if (!eventManager)
        {
          Debug.LogError("There needs to be one active MovementEventManager script on a GameObject in your scene.");
        }
        else
        {
          eventManager.Init();
        }
      }
      return eventManager;
    }
  }

  void Init()
  {
    if (eventDictionary == null)
    {
      eventDictionary = new Dictionary<string, Action<float>>();
    }
  }

  public static void StartListening(string eventName, Action<float> listener)
  {
    Action<float> thisEvent;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
    {
      thisEvent += listener;
    }
    else
    {
      thisEvent += listener;
      instance.eventDictionary.Add(eventName, thisEvent);
    }
  }

  public static void StopListening(string eventName, Action<float> listener)
  {
    if (eventManager == null) return;
    Action<float> thisEvent;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
    {
      thisEvent -= listener;
    }
  }

  public static void TriggerEvent(string eventName, float h)
  {
    Action<float> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
    {
      thisEvent.Invoke(h);
    }
  }
}

