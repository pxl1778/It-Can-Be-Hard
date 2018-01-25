using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingCharacter : MonoBehaviour {
    [SerializeField]
    GameObject pathObject;
    [SerializeField]
    float maxTimer = 1.0f;
    float timer = 0;
    int currentPoint = 0;
    bool moving = false;
    private Vector3 originalPoint;
    PathPoint[] path;


	// Use this for initialization
	void Start () {
        path = pathObject.GetComponentsInChildren<PathPoint>();
	}
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            timer += Time.deltaTime;
            this.transform.position = Vector3.Lerp(originalPoint, path[currentPoint].transform.position, calcEase(timer / maxTimer));
            if (timer >= maxTimer)
            {
                timer = 0;
                moving = false;
                currentPoint++;
            }
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (moving == false && currentPoint < path.Length)
            {
                moving = true;
                maxTimer = path[currentPoint].Speed;
                originalPoint = this.transform.position;
            }
        }
    }

    public float calcEase(float pAlpha)
    {
        return pAlpha * pAlpha * (3.0f - 2.0f * pAlpha);
    }
}
