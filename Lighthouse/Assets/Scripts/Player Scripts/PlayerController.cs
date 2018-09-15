using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private float acceleration;
    private float turnSpeed = 650.0f;
    private float timer = 0;
    private float delay = 0;
    
    private Rigidbody rb;
    private GameObject cam;
    private GameManager gm;
    private Animator anim;
    private PlayerBubble bubble;
    [SerializeField]
    private Cinemachine.CinemachineFreeLook playerCam;

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
        gm.EventMan.movePlayerToPosition.AddListener(StartMoveToPosition);
        bubble = this.GetComponentInChildren<PlayerBubble>();
        if(bubble != null)
        {
            bubble.Deactivate();
        }
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void FixedUpdate()
    {
        if(gm.Player == null)
        {//making sure that game manager has the player
            gm.Player = this.GetComponent<Player>();
        }
        if(gm.Player.State == PlayerState.ACTIVE)
        {
            //if (Input.GetButton("Fire1"))
            //{
            //    acceleration = 7;
            //}
            //else
            //{
                acceleration = 5;
            //}
            if (Input.GetButtonDown("Fire3") && bubble!=null && !bubble.enabled)
            {
                Debug.Log("shift pressed");
                bubble.enabled = true;
                bubble.Activate();
            }

            //controlling camera when mouse is down
            if (Input.GetMouseButton(0))
            {
                playerCam.m_XAxis.m_InputAxisName = "Mouse X";
                playerCam.m_YAxis.m_InputAxisName = "Mouse Y";
            }
            else
            {
                playerCam.m_XAxis.m_InputAxisName = "";
                playerCam.m_YAxis.m_InputAxisName = "";
                playerCam.m_XAxis.m_InputAxisValue = 0;
                playerCam.m_YAxis.m_InputAxisValue = 0;
            }
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
            Vector3 newVelocity = Vector3.ClampMagnitude(m_Move, 1) * acceleration;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("sitting"))
            {
                rb.velocity = new Vector3(newVelocity.x, Mathf.Clamp(rb.velocity.y, -100, 1), newVelocity.z);
                if (m_Move.x != 0 || m_Move.y != 0)
                {
                    this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(m_Move), turnSpeed * Time.deltaTime);
                }
            }

        }
        else if (gm.Player.State == PlayerState.MOVING)
        {
            MoveToPosition();
        }
        else if(gm.Player.State == PlayerState.CUTSCENE)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            Debug.Log("in cutscene");
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            anim.SetFloat("Velocity", 0);
        }
    }

    public void StopPlayer()
    {
        rb.velocity = Vector3.zero;
        anim.SetFloat("Velocity", 0);
    }

    public void StartMoveToPosition(Vector3 pPosition, float delay, bool pIsRunning)
    {
        Debug.Log("Started Moving");
        targetPosition = pPosition;
        isRunning = pIsRunning;
        timer = 0;
        this.delay = delay;
        StartCoroutine(delayWait());
    }

    public void MoveToPosition()
    {
        if(timer >= delay)
        {
            Vector3 m_Move = Vector3.Scale(Vector3.Normalize(targetPosition - this.transform.position), new Vector3(1, 0, 1)).normalized;
            float speed = (isRunning) ? 5.0f : 1.6f;
            float animSpeed = (isRunning) ? 1.0f : .6f;//maybe ease this so it doesn't cut off so quickly at end 
            anim.SetFloat("Velocity", m_Move.magnitude * animSpeed);
            Vector3 newVelocity = Vector3.ClampMagnitude(m_Move, 1) * speed;
            rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);

            if (m_Move.x != 0 || m_Move.y != 0)
            {
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(m_Move), turnSpeed * Time.deltaTime);
            }

            if ((this.transform.position - targetPosition).magnitude < .6)
            {
                this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
                gm.Player.State = PlayerState.INACTIVE;
                gm.EventMan.finishedLerp.Invoke("PlayerNodes");
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    IEnumerator delayWait()
    {
        while(timer<delay)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        gm.Player.State = PlayerState.MOVING;
    }
}
