﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialogueBox : MonoBehaviour {

    private RectTransform rt;
    private Camera cam;
    //private Transform target;
    private RectTransform canvasRect;
    private Text text;
    [SerializeField]
    private float followSpeed = 1.0f;
    [SerializeField]
    private Transform target;

    // Use this for initialization
    void Start()
    {
        rt = this.GetComponent<RectTransform>();
        cam = Camera.main;
        //target = GameObject.Find("Player").transform;
        canvasRect = this.transform.parent.GetComponent<RectTransform>();
        text = this.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //rt.transform.forward = cam.transform.forward;
        if(target == null)
        {
            return;
        }
        Vector3 targetPos = new Vector3(target.position.x, target.position.y + 0.3f, target.position.z);
        Vector2 viewportPoint = cam.WorldToViewportPoint(targetPos);
        Vector2 desiredPoint = new Vector3(viewportPoint.x * canvasRect.sizeDelta.x, (viewportPoint.y) * canvasRect.sizeDelta.y, 0) * canvasRect.localScale.x;
        
        rt.position = new Vector3(rt.position.x + ((desiredPoint.x - rt.position.x) * followSpeed), rt.position.y + ((desiredPoint.y - rt.position.y) * followSpeed), 0);
        //rt.position = new Vector3(desiredPoint.x, desiredPoint.y, 0);
        if (rt.position.x < 100)
        {
            rt.position = new Vector3(100, rt.position.y, rt.position.z);
        }
        if (rt.position.x > (canvasRect.sizeDelta.x * canvasRect.transform.localScale.x) - 100)
        {
            rt.position = new Vector3((canvasRect.sizeDelta.x * canvasRect.transform.localScale.x) - 100, rt.position.y, rt.position.z);
        }
        if (rt.position.y < 100)
        {
            rt.position = new Vector3(rt.position.x, 100, rt.position.z);
        }
        if (rt.position.y > (canvasRect.sizeDelta.y * canvasRect.transform.localScale.y) - 100)
        {
            rt.position = new Vector3(rt.position.x, (canvasRect.sizeDelta.y * canvasRect.transform.localScale.y) - 100, rt.position.z);
        }
        ////rt.transform.forward = cam.transform.forward;
        //Vector3 targetPos = new Vector3(target.position.x, target.position.y + (3.0f * canvasRect.localScale.x), target.position.z);
        //Vector2 viewportPoint = cam.WorldToViewportPoint(targetPos);
        //Vector2 desiredPoint = new Vector2(viewportPoint.x * canvasRect.sizeDelta.x, viewportPoint.y * canvasRect.sizeDelta.y) * canvasRect.localScale.x;

        ////Vector2 desiredPoint = new Vector2(viewportPoint.x, viewportPoint.y);
        //rt.position = new Vector3(rt.position.x + ((desiredPoint.x - rt.position.x)*followSpeed), rt.position.y + ((desiredPoint.y - rt.position.y)*followSpeed), 0);
        ////rt.position = new Vector3(desiredPoint.x, desiredPoint.y, 0);
        //if (rt.position.x < 200)
        //{
        //    rt.position = new Vector3(200, rt.position.y, rt.position.z);
        //}
        //if (rt.position.x > canvasRect.sizeDelta.x - 200)
        //{
        //    rt.position = new Vector3(canvasRect.sizeDelta.x - 200, rt.position.y, rt.position.z);
        //}
        //if (rt.position.y < 50)
        //{
        //    rt.position = new Vector3(rt.position.x, 50, rt.position.z);
        //}
        //if (rt.position.y > canvasRect.sizeDelta.y - 50)
        //{
        //    rt.position = new Vector3(rt.position.x, canvasRect.sizeDelta.y - 50, rt.position.z);
        //}
    }
}
