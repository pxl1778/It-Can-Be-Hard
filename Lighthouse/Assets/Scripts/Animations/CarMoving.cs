using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoving : MonoBehaviour {

    private Animator anim;
    private float speed = 1.0f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool rotateCar = false;
    [SerializeField]
    private float waitTime = 0.0f;
    private float elapsedTime = 0.0f;
    private bool active = false;
    private float originalSpeed = 1.0f;

	// Use this for initialization
	void Start () {
        anim = this.transform.GetComponent<Animator>();
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        originalSpeed = anim.speed;
        anim.speed = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (rotateCar)
        {
            rotateCar = false;
            this.transform.SetPositionAndRotation(originalPosition, originalRotation);
        }
        if (!active)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > waitTime)
            {
                active = true;
                anim.speed = originalSpeed;
            }
        }
        else
        {
            anim.speed = speed;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StopCoroutine("LerpSpeed");
            StartCoroutine(LerpSpeed(speed, 0.0f, 0.5f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StopCoroutine("LerpSpeed");
            StartCoroutine(LerpSpeed(speed, 1.0f, 0.3f));
        }
    }

    public void Intersection(int num)
    {
        anim.SetInteger("Direction", num);
        rotateCar = true;
    }

    IEnumerator LerpSpeed(float a, float b, float duration)
    {
        float elapsedTime = 0.0f;
        float alpha = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            alpha = elapsedTime / duration;
            speed = Mathf.Lerp(a, b, alpha);
            yield return new WaitForEndOfFrame();
        }
    }
}
