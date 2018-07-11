using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringFloat2UnityEvent : UnityEvent<string, float, float> { }
[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }
[System.Serializable]
public class StringArrayUnityEvent : UnityEvent<string[]> { }
[System.Serializable]
public class Vector3UnityEvent : UnityEvent<Vector3,float, bool> { }
[System.Serializable]
public class TransformUnityEvent : UnityEvent<Transform, float, float> { }
[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> { }

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
    public StringFloat2UnityEvent lerpGlobalValue = new StringFloat2UnityEvent();
    public UnityEvent endCutscene = new UnityEvent();
    public FloatUnityEvent changePlayerFace = new FloatUnityEvent();

    //Dialogue Events
    public StringArrayUnityEvent startPlayerDialogue = new StringArrayUnityEvent();
    public UnityEvent endPlayerDialogue = new UnityEvent();
}
