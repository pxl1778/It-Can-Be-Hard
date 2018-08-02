using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptionsBox : MonoBehaviour
{

    private RectTransform rt;
    private Camera cam;
    private Transform target;
    private RectTransform canvasRect;
    private Text text;
    private RectTransform otherBox;
    public RectTransform OtherBox { set { otherBox = value; } }
    [SerializeField]
    private float followSpeed = 1.0f;
    
    void Start()
    {
        rt = this.GetComponent<RectTransform>();
        cam = Camera.main;
        target = GameObject.Find("Player").transform;
        canvasRect = this.transform.parent.GetComponent<RectTransform>();
        text = this.GetComponentInChildren<Text>();
    }
    
    void Update()
    {
        //Vector3 targetPos = new Vector3(target.position.x, target.position.y + 2.0f, target.position.z);//little above target
        //Vector2 viewportPoint = cam.WorldToViewportPoint(targetPos);//that point on screen
        //Vector2 desiredPoint = new Vector2(viewportPoint.x * canvasRect.sizeDelta.x, viewportPoint.y * canvasRect.sizeDelta.y);//where we want it to be (jitters otherwise)
        //desiredPoint = new Vector2(otherBox.position.x + (otherBox.sizeDelta.x * otherBox.localScale.x)/2 - rt.sizeDelta.x/2, otherBox.position.y - ((otherBox.sizeDelta.y*otherBox.localScale.x / 2) + (rt.sizeDelta.y / 2)));
        //rt.position = new Vector3(rt.position.x + ((desiredPoint.x - rt.position.x) * followSpeed), rt.position.y + ((desiredPoint.y - rt.position.y) * followSpeed), 0);

        ////making sure it doesn't go off screen
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

        Vector3 targetPos = new Vector3(target.position.x, target.position.y + (1.5f * canvasRect.localScale.x), target.position.z);
        Vector2 viewportPoint = cam.WorldToViewportPoint(targetPos);
        rt.position = new Vector3(viewportPoint.x * canvasRect.sizeDelta.x, viewportPoint.y * canvasRect.sizeDelta.y, 0) * canvasRect.localScale.x;
        if (rt.position.x < 200)
        {
            rt.position = new Vector3(200, rt.position.y, rt.position.z);
        }
        if (rt.position.x > canvasRect.sizeDelta.x - 200)
        {
            rt.position = new Vector3(canvasRect.sizeDelta.x - 200, rt.position.y, rt.position.z);
        }
        if (rt.position.y < 100)
        {
            rt.position = new Vector3(rt.position.x, 100, rt.position.z);
        }
        if (rt.position.y > canvasRect.sizeDelta.y - 100)
        {
            rt.position = new Vector3(rt.position.x, canvasRect.sizeDelta.y - 100, rt.position.z);
        }

        //Canvas.ForceUpdateCanvases();
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x, 90 * text.cachedTextGenerator.lines.Count);
    }

}
