using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    public float acceleration;
    
    private Rigidbody rb;
    public GameObject cam;

	// Use this for initialization
	void Start () {
        rb = this.GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //get the horizontal and vertical input components
        float h = Input.GetAxis("Horizontal") * acceleration;
        float v = Input.GetAxis("Vertical") * acceleration;

        //create velocity vector
        Vector3 velocity = new Vector3(v, rb.velocity.y, -h);
        float angle = Vector3.Angle(Vector3.forward, Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up));
        //rotate in direction of camera
        velocity = Quaternion.AngleAxis(angle, Vector3.up) * velocity;

        //set the velocity
        rb.velocity = velocity;
	}

    void FixedUpdate()
    {
        if(Input.GetButton("Fire1"))
        {
            acceleration = 10;
        }
        else
        {
            acceleration = 5;
        }
    }
}
