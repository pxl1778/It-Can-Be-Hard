using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private bool active = false;
    private Canvas endDayCanvas;

	// Use this for initialization
	void Start () {
        endDayCanvas = GameObject.Find("EndDayCanvas").GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active && Input.GetKeyDown(KeyCode.Space))
        {
            endDayCanvas.enabled = true;
            active = false;
            GameManager.instance.Player.State = PlayerState.INACTIVE;
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && (bool)GameManager.instance.Globals.Dictionary["doorActive"])
        {
            active = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            active = false;
        }
    }
}
