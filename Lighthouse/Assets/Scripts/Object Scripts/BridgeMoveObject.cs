using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeMoveObject : MonoBehaviour {

    private Vector3 endPos = new Vector3(-16.72f, -0.51f, 47.05f);
    private Vector3 startPos = new Vector3(-13.99f, -0.51f, 47.25f);
    private float alpha = 0;
    private bool active = false;
    private bool standingOn = false;
    private bool dragging = false;
    private float prevX = 0;
    private float prevY = 0;
    private Color originalColor;
    private Vector3 moveDirection;
    private GameManager gm;

    // Use this for initialization
    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && active)
        {//check if clicked object
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = (1 << 8);
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if (hit.transform == this.transform)
                {
                    dragging = true;
                    gm.Player.State = PlayerState.INACTIVE;
                    prevX = Input.mousePosition.x;
                    prevY = Input.mousePosition.y;
                    Vector3 tStartPos = Camera.main.WorldToScreenPoint(startPos);
                    Vector3 tEndPos = Camera.main.WorldToScreenPoint(endPos);
                    moveDirection = Vector3.Normalize(tEndPos - tStartPos);
                }
            }
        }
        if (dragging && Input.GetMouseButton(0))
        {//use normalized direction of start and end point to drag object
            float xAmount = (prevX * 0.001f - Input.mousePosition.x * 0.001f) * moveDirection.x * -1;
            float yAmount = (prevY * 0.001f - Input.mousePosition.y * 0.001f) * moveDirection.y * -1;
            alpha = Mathf.Clamp01(alpha + xAmount + yAmount);
            this.transform.localPosition = Vector3.Lerp(startPos, endPos, alpha);
            prevX = Input.mousePosition.x;
            prevY = Input.mousePosition.y;
        }
        else if(dragging && !Input.GetMouseButton(0))
        {//reset after fininshing dragging
            gm.Player.State = PlayerState.ACTIVE;
            DeactivateObject();
        }
    }

    public void DeactivateObject()
    {
        dragging = false;
        active = false;
        Material m = GetComponent<MeshRenderer>().material;
        m.SetColor("_Color", originalColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBubble>() && !standingOn)
        {//check if you touched the object WITH YOUR MIND
            active = true;
            Material m = GetComponent<MeshRenderer>().material;
            originalColor = m.color;
            m.SetColor("_Color", Color.blue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            standingOn = true;
            DeactivateObject();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            standingOn = false;
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("you hit it");
    }
}
