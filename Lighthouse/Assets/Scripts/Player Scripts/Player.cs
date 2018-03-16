﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    ACTIVE, INACTIVE, MOVING
}

public class Player : MonoBehaviour {
	private string playerName = "Luke";
    private PlayerState state = PlayerState.ACTIVE;
    public PlayerState State { get { return state; } set { state = value; } }
	//private GameManager gm;
	// Use this for initialization
	void Awake () {
		//gm = (GameManager)(GameObject.Find ("GameManager").GetComponent<GameManager> ());
	}
}