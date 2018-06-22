using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpeechBubble : MonoBehaviour {

    private Camera cam;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.up = -cam.transform.forward;
        this.transform.eulerAngles = new Vector3(100, cam.transform.eulerAngles.y, 180);
    }
}
