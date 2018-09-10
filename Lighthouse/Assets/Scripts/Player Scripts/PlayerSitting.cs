using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitting : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Animator>().SetBool("Sitting", true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
