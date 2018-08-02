using UnityEngine;

public class birdController : MonoBehaviour {

    private float timePassed;
    private float timeTilJump;
    private bool move = false;
    private Animator anim;
    private bool flying = false;

	// Use this for initialization
	void Start () {
        timePassed = 0;
        timeTilJump = Random.Range(4, 7);
        anim = this.transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;
        anim.SetFloat("time", timePassed / timeTilJump);
        if (timePassed >= timeTilJump)
        {
            timePassed = 0;
            move = false;
            timeTilJump = Random.Range(4, 7);
        }
	}

    private void LateUpdate()
    {
        if(!move && timePassed/timeTilJump >= 0.8)
        {
            move = true;
            if(Random.Range(0, 10) >= 5)
            {
                transform.Rotate(Vector3.up, 100);
                anim.SetBool("move", true);
            }
            else
            {
                anim.SetBool("move", false);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetBool("fly", true);
            flying = true;
            Vector3 direction = other.gameObject.transform.position - transform.position;
            this.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(new Vector3(direction.x, 0, direction.z)));
        }
    }
}
