using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private Player playerInfo;

	public Player PlayerInfo{ get { return playerInfo; } set { playerInfo = value; } }

	void Awake(){
		if (instance == null) {
			instance = this;
			playerInfo = (Player)(GameObject.Find("Player").GetComponent<Player>());
		} else if (instance != null) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
