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
    GameManager gm;
    //lerp in cutscene
    Vector3 target;
    float lerpDelay;
    bool inCutscene = false;
    [SerializeField]
    private AudioSource moveSound;

	// Use this for initialization
	void Start () {
        path = pathObject.GetComponentsInChildren<PathPoint>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (inCutscene)
        {
            timer += Time.deltaTime;
            if(timer >= 0)
            {
                this.transform.position = Vector3.Lerp(originalPoint, target, calcEase(timer / maxTimer));
            }
            if (timer >= maxTimer)
            {
                timer = 0;
                inCutscene = false;
                moving = false;
                currentPoint++;
                gm.EventMan.finishedLerp.Invoke("DogNodes");
            }
        }
        else
        {
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
                moveSound.pitch = Random.Range(0.9f, 1.1f);
                moveSound.PlayOneShot(moveSound.clip);
            }
            if(currentPoint >= path.Length -1)
            {
                StartCoroutine(LerpScaleDown());
            }
        }
    }

    IEnumerator LerpScaleDown()
    {
        float lerpTime = 0;
        Vector3 originalScale = this.transform.localScale;
        while(lerpTime < 0.7f)
        {
            lerpTime += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(originalScale, new Vector3(0.01f, 0.01f, 0.01f), lerpTime / 0.7f);
            yield return new WaitForEndOfFrame();
        }
        this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    public float calcEase(float pAlpha)
    {
        return pAlpha * pAlpha * (3.0f - 2.0f * pAlpha);
    }

    public void MoveToFinalPosition()
    {
        moving = true;
        maxTimer = path[path.Length - 1].Speed;
        originalPoint = this.transform.position;
        currentPoint = path.Length - 1;
        StartCoroutine(LerpScaleDown());
    }

    public void StartMoveToPosition(Vector3 pPosition, float pDelay, float pDuration)
    {
        target = pPosition;
        lerpDelay = pDelay;
        maxTimer = pDuration;
        timer = pDelay * -1;
        originalPoint = this.transform.position;
        inCutscene = true;
        moving = false;
    }

    public void DoParticles1()
    {
        ParticleSystem.EmissionModule em = this.GetComponentInChildren<ParticleSystem>().emission;
        em.rateOverTime = 100.0f;
        ParticleSystem.MainModule mm = this.GetComponentInChildren<ParticleSystem>().main;
        mm.startSpeed = 1.0f;
    }

    public void DoParticles2()
    {
        ParticleSystem[] psArray = this.GetComponentsInChildren<ParticleSystem>();
        psArray[1].Play();
        psArray[0].Stop();
        MeshRenderer[] meshArray = GameObject.Find("Invisible").GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshArray.Length; i++)
        {
            meshArray[i].enabled = true;
        }
        GameManager.instance.EventMan.lerpGlobalValue.Invoke(GlobalData.GLOW_RADIUS, 10, 2);
    }
}
