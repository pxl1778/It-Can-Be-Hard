using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    private float acceleration;
    private float turnSpeed = 650.0f;
    
    private Rigidbody rb;
    private GameObject cam;
    private GameManager gm;
    private Animator anim;

    //lerping
    private Vector3 targetPosition;
    private bool isRunning;
    public delegate void finishLerp();

    // Use this for initialization
    void Start () {
        rb = this.GetComponentInParent<Rigidbody>();
        cam = Camera.main.gameObject;
        anim = this.GetComponent<Animator>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        
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
        if(gm.Player.State == PlayerState.ACTIVE)
        {

            //get the horizontal and vertical input components
            float h = Input.GetAxis("Horizontal");
            h = (Mathf.Abs(h) < .1f) ? 0 : h;
            float v = Input.GetAxis("Vertical");
            v = (Mathf.Abs(v) < .1f) ? 0 : v;

            //create velocity vector
            Vector3 m_CamForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;

            if (cam.transform.rotation.y == 0)//checking weird quaternion forward issue
            {
                m_CamForward.x += .001f;
            }
            Vector3 m_Move = v * m_CamForward + h * cam.transform.right;
            anim.SetFloat("Velocity", m_Move.magnitude);

            //set the velocity
            rb.velocity = Vector3.ClampMagnitude(m_Move, 1) * acceleration;

            if (m_Move.x != 0 || m_Move.y != 0)
            {
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(m_Move), turnSpeed * Time.deltaTime);
            }
        }
    }

    public void StopPlayer()
    {
        rb.velocity = Vector3.zero;
    }

    public void StartMoveToPosition(Vector3 pPosition, bool pIsRunning)
    {
        targetPosition = pPosition;
        isRunning = pIsRunning;
        gm.Player.State = PlayerState.MOVING;
    }

    public void MoveToPosition()
    {
        Vector3 m_CamForward = Vector3.Scale(Vector3.Normalize(targetPosition - this.transform.position), new Vector3(1, 0, 1)).normalized;
        float speed = (isRunning) ? 1.0f : .3f;
        Vector3 m_Move = speed * m_CamForward;
        //m_Character.Move(m_Move, false, false);
        rb.velocity = m_Move * acceleration;
        if ((this.transform.position - targetPosition).magnitude < .7)
        {
            gm.Player.State = PlayerState.INACTIVE;
            gm.EventMan.finishedLerp.Invoke("PlayerNodes");
        }
    }
}
