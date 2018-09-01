using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoving : MonoBehaviour {

    private Animator anim;
    private float speed = 1.0f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool rotateCar = false;

	// Use this for initialization
	void Start () {
        anim = this.transform.GetComponent<Animator>();
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        anim.speed = speed;
        if (rotateCar)
        {
            rotateCar = false;
            this.transform.SetPositionAndRotation(originalPosition, originalRotation);
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
