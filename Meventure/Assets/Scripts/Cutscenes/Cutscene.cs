﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour {

    protected GameManager gm;

	// Use this for initialization
	void Start ()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartCutscene()
    {
        Debug.Log(gm);
        gm.PlayerInfo.State = PlayerState.INACTIVE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCutscene();
        }
    }

    protected virtual void PlayCutscene()
    {
        //camera lerping
        //characters moving
        //graphical changes
        //type of lerp, length of time of lerp, thing to happen after lerp is done
    }
}