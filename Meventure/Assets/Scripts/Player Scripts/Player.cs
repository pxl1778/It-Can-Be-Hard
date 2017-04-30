using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public string name = "Dan";
	private GameManager gm;
	// Use this for initialization
	void Awake () {
		gm = (GameManager)(GameObject.Find ("GameManager").GetComponent<GameManager> ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
