using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringUnityEvent : UnityEvent<string, float, float> { }
[System.Serializable]
public class Vector3UnityEvent : UnityEvent<Vector3, float, float> { }

public class EventManager : MonoBehaviour {
    //Loading events
    public UnityEvent dialogueLoadFinished = new UnityEvent();

    //Camera events
    public StringUnityEvent lerpToTarget = new StringUnityEvent();
    public UnityEvent lookAtPlayer = new UnityEvent();
    public UnityEvent stopPlayer = new UnityEvent();
    public Vector3UnityEvent movePlayerToPosition = new Vector3UnityEvent();
}
