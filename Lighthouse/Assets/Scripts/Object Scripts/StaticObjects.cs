using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjects : MonoBehaviour {

    private bool selectable = false;
    private bool active = false;
    private Color originalColor;
    private Material mat;
    private Rigidbody rb;
    [SerializeField]
    private ParticleSystem hitParticles;
    [SerializeField]
    private ParticleSystem tkParticle;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
        tkParticle = this.GetComponentInChildren<ParticleSystem>();
        mat = this.GetComponent<MeshRenderer>().material;
        originalColor = mat.GetColor("_EffectColor");
        mat.SetColor("_EffectColor", Color.black);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && selectable)
        {//check if clicked object
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = (1 << 8);
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if (hit.transform == this.transform)
                {//clicked on object
                    selectable = false;
                    active = false;
                    tkParticle.Stop();
                    mat.SetColor("_EffectColor", Color.black);
                    Vector3 force = this.transform.position - GameManager.instance.Player.transform.position;
                    rb.isKinematic = false;
                    rb.AddForceAtPosition(force * 30, this.transform.position + new Vector3(0.0f, 0.1f, 0.0f), ForceMode.Force);
                    hitParticles.Play();
                }
            }
        }
    }

    public void DeactivateObject()
    {
        active = false;
        selectable = false;
        Material m = GetComponent<MeshRenderer>().material;
        mat.SetColor("_EffectColor", Color.black);
        tkParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBubble>() && this.enabled)
        {//check if you touched the object WITH YOUR MIND
            selectable = true;
            tkParticle.Play();
            mat.SetColor("_EffectColor", originalColor);
        }
    }
}
