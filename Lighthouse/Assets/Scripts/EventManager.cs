using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringFloat2UnityEvent : UnityEvent<string, float, float> { }
[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }
[System.Serializable]
public class Vector3UnityEvent : UnityEvent<Vector3, bool> { }
[System.Serializable]
public class TransformUnityEvent : UnityEvent<Transform, float, float> { }

public class EventManager : MonoBehaviour {
    //Loading events
    public UnityEvent dialogueLoadFinished = new UnityEvent();

    //Camera events
    public StringFloat2UnityEvent lerpToTarget = new StringFloat2UnityEvent();
    public StringUnityEvent finishedLerp = new StringUnityEvent();
    public UnityEvent lookAtPlayer = new UnityEvent();
    public UnityEvent stopPlayer = new UnityEvent();
    public Vector3UnityEvent movePlayerToPosition = new Vector3UnityEvent();
    public TransformUnityEvent lerpCameraToTransform = new TransformUnityEvent();
}
