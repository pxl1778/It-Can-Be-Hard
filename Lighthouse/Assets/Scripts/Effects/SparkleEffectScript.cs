using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class SparkleEffectScript : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = .2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
	}
}
