using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueBox : MonoBehaviour {

    private RectTransform rt;
    private Camera cam;
    private Transform target;
    private RectTransform canvasRect;
    private Text text;

	// Use this for initialization
	void Start () {
        rt = this.GetComponent<RectTransform>();
        cam = Camera.main;
        target = this.transform.parent.transform.parent.GetComponentInChildren<Animator>().transform;
        canvasRect = this.transform.parent.GetComponent<RectTransform>();
        text = this.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        //rt.transform.forward = cam.transform.forward;
        Vector3 targetPos = new Vector3(target.position.x, target.position.y + (1.5f * canvasRect.localScale.x), target.position.z);
        Vector2 viewportPoint = cam.WorldToViewportPoint(targetPos);

        //Debug.Log(canvasRect.sizeDelta.x);
        rt.position = new Vector3(viewportPoint.x * canvasRect.sizeDelta.x, viewportPoint.y * canvasRect.sizeDelta.y, 0) * canvasRect.localScale.x;
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

        //Canvas.ForceUpdateCanvases();
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x, 130 * text.cachedTextGenerator.lines.Count);
    }
}
