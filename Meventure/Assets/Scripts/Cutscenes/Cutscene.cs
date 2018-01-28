using System.Collections;
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

    protected virtual void StartCutscene()
    {
        gm.PlayerInfo.State = PlayerState.INACTIVE;
        gm.EventMan.stopPlayer.Invoke();
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
    }
}
