using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    ACTIVE, INACTIVE
}

public class Player : MonoBehaviour {
	public string playerName = "Dan";
    private PlayerState state = PlayerState.ACTIVE;
    public PlayerState State { get { return state; } set { state = value; } }
	private GameManager gm;
	// Use this for initialization
	void Awake () {
		gm = (GameManager)(GameObject.Find ("GameManager").GetComponent<GameManager> ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
