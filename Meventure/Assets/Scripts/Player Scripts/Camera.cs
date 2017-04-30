using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public Vector3 normalDirection;
    public float distance;
    public float maxForce;
    public float tightness;

    private GameObject player;
    private Vector3 camForward;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").gameObject;
        camForward = this.transform.forward;
        rb = this.GetComponent<Rigidbody>();

        //setting initial camera location
        normalDirection = normalDirection.normalized;
        Vector3 target = player.transform.position;
        this.transform.position = target - (normalDirection * distance);

        transform.LookAt(target);
    }
	
	// Update is called once per frame
	void Update () {
        /*//get target and offset of the camera
        Vector3 target = player.transform.position;
        Vector3 offset = camForward * distance;

        //change position
        transform.position = target - offset;
        //make camera look at the player
        transform.LookAt(target);*/
        normalDirection = normalDirection.normalized;
        Vector3 target = player.transform.position;
        Vector3 desiredLocation = target - (normalDirection * distance);

        Vector3 distanceVector = desiredLocation - this.transform.position;
        distanceVector = Vector3.ClampMagnitude(distanceVector, maxForce);
        Vector3 force = distanceVector - this.rb.velocity;

        rb.AddForce(force);

        transform.LookAt(target);
    }
}
