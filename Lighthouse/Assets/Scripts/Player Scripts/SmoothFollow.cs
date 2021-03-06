﻿using UnityEngine;

public enum CameraState
{
    PLAYERFOCUS, LERPTOTARGET, LERPCOMPLETED, LERPTOPLAYER, LERPTONODE
}

public class SmoothFollow : MonoBehaviour
{

	// The target we are following
	[SerializeField]
	private Transform target;
	// The distance in the x-z plane to the target
	[SerializeField]
	private float distance = 10.0f;
	// the height we want the camera to be above the target
	[SerializeField]
	private float height = 5.0f;

	[SerializeField]
	private float rotationDamping;
	[SerializeField]
	private float heightDamping;
    [SerializeField]
    private float translateDamping;

    [SerializeField]
    private float rotation;

    private GameManager gm;
    private CameraState state = CameraState.PLAYERFOCUS;
    //for lerping
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float lerpDuration;
    private float lerpAlpha=0f;
    private float lerpDelay=0f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Quaternion originalPlayerRotation;
    public delegate void finishLerp();

    // Use this for initialization
    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.EventMan.lookAtPlayer.AddListener(LookAtPlayer);
        gm.EventMan.lerpToTarget.AddListener(LerpToTarget);
        gm.EventMan.lerpCameraToTransform.AddListener(StartLerpToNode);
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }

    private void Update()
    {
        if(gm.Player.State == PlayerState.ACTIVE)
        {
            if(Input.GetMouseButton(0))
            {
                rotation += Input.GetAxis("Mouse X") * 2;
                height += Input.GetAxis("Mouse Y");
                height = Mathf.Clamp(height, 3, 6);
            }
            rotation -= Input.GetAxis("Joy X");
        }
    }

    // Update is called once per frame
    void LateUpdate()
	{
        switch (state)
        {
            case CameraState.PLAYERFOCUS:
                playerFocus();
                break;
            case CameraState.LERPTOTARGET:
                lerp();
                break;
            case CameraState.LERPTOPLAYER:
                LerpToPlayer();
                break;
            case CameraState.LERPTONODE:
                LerpToNode();
                break;
            default:
                break;
        }
		
	}

    private void playerFocus()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        // Calculate the current rotation angles
        var wantedRotationAngle = rotation;
        var wantedHeight = target.position.y + height;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        //currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentRotationAngle = wantedRotationAngle;

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        Vector3 tTargetPosition = target.position;
        //transform.position = target.position;
        tTargetPosition -= currentRotation * Vector3.forward * distance;
        tTargetPosition = new Vector3(tTargetPosition.x, currentHeight, tTargetPosition.z);

        // Set the height of the camera
        //transform.position = Vector3.Lerp(transform.position, tTargetPosition, translateDamping * Time.deltaTime);
        transform.position = tTargetPosition;
        Ray ray = new Ray(target.position, Camera.main.transform.position - target.position );
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
            //transform.position = hit.point;
        }

        // Always look at the target
        transform.LookAt(target);
    }

    public void lerp()
    {
        lerpAlpha = lerpAlpha + Time.deltaTime;
        //lerpAlpha = lerpAlpha * lerpAlpha * (3.0f - 2.0f * lerpAlpha);
        Debug.Log("[lerpToTarget]lerpalpha: " + lerpAlpha);
        transform.position = Vector3.Lerp(originalPosition, targetPosition, calcEase(lerpAlpha/lerpDuration));
        transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, calcEase(lerpAlpha/lerpDuration));
        if(lerpAlpha/lerpDuration >= 1)
        {
            Debug.Log("end of lerp");
            lerpAlpha = 0;
            state = CameraState.LERPCOMPLETED;
        }
    }

    public float calcEase(float pAlpha)
    {
        return pAlpha * pAlpha * (3.0f - (2.0f * pAlpha));
    }

    public void LerpToPlayer()
    {
        lerpAlpha = lerpAlpha + Time.deltaTime;
        transform.position = Vector3.Lerp(originalPosition, targetPosition, calcEase(lerpAlpha / lerpDuration));
        transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, calcEase(lerpAlpha / lerpDuration));
        if (lerpAlpha >= 1)
        {
            lerpAlpha = 0;
            state = CameraState.PLAYERFOCUS;
        }
    }

    public void LerpToTarget(string pTargetName, float pRotation, float pDuration)
    {
        Debug.Log("trying to change target: " + pTargetName);
        GameObject tTarget = GameObject.Find(pTargetName);
        if (tTarget != null)
        {
            Debug.Log("found target");
            if(state == CameraState.PLAYERFOCUS)
            {
                originalPlayerRotation = transform.rotation;
            }
            target = tTarget.transform;
            targetPosition = target.position;
           
            var currentHeight = target.position.y + height;
            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0, pRotation, 0);

            targetPosition -= currentRotation * Vector3.forward * distance;
            targetPosition = new Vector3(targetPosition.x, currentHeight, targetPosition.z);

            //get targetRotation
            targetRotation = Quaternion.LookRotation(target.position - targetPosition);

            originalPosition = transform.position;
            originalRotation = transform.rotation;

            lerpDuration = pDuration;

            state = CameraState.LERPTOTARGET;
        }
    }

    public void LookAtPlayer()
    {
        state = CameraState.LERPTOPLAYER;

        target = GameObject.Find("Player").transform;
        targetPosition = target.position;
        float currentHeight = target.position.y + height;
        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, rotation, 0);
        targetPosition -= currentRotation * Vector3.forward * distance;
        targetPosition = new Vector3(targetPosition.x, currentHeight, targetPosition.z);

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        //get it
        Quaternion rot = Quaternion.LookRotation(Vector3.Normalize(target.position - targetPosition));
        //done getting it
        targetRotation = rot;
        lerpDuration = 1.0f;
    }

    public void StartLerpToNode(Transform pTransform, float pDelay, float pDuration)
    {
        state = CameraState.LERPTONODE;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        target = pTransform;
        targetRotation = pTransform.rotation;
        lerpDuration = pDuration;
        lerpDelay = pDelay;
    }

    public void LerpToNode()
    {
        lerpAlpha = lerpAlpha + Time.deltaTime - lerpDelay;
        if(lerpAlpha > 0)
        {
            float clampedAlpha = Mathf.Clamp01(lerpAlpha / lerpDuration);
            transform.position = Vector3.Lerp(originalPosition, target.position, calcEase(clampedAlpha));
            transform.rotation = Quaternion.Slerp(originalRotation, target.rotation, calcEase(clampedAlpha));
            if (clampedAlpha >= 1)
            {
                Debug.Log("end of cam lerp to node");
                lerpAlpha = 0;
                state = CameraState.LERPCOMPLETED;
                gm.EventMan.finishedLerp.Invoke("CameraNodes");
            }
        }
    }
}
