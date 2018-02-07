using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour {

    private RectTransform rt;
    private Camera cam;

	// Use this for initialization
	void Start () {
        rt = this.GetComponent<RectTransform>();
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        rt.transform.forward = cam.transform.forward;
	}
}
