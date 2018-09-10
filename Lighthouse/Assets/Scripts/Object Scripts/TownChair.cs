using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownChair : MonoBehaviour {

    private float alpha = 0;
    private bool selectable = false;
    private bool active = false;
    private ParticleSystem tkParticle;
    private Material mat;
    private Rigidbody rb;
    [SerializeField]
    private Conversation convo;

    private void Start()
    {
        tkParticle = transform.GetComponentInChildren<ParticleSystem>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
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
                    active = true;
                    active = false;
                    tkParticle.Stop();
                    mat.SetColor("_RimColor", new Vector4(0, 0, 0, 1));
                    Vector3 force = this.transform.position - GameManager.instance.Player.transform.position;
                    //rb.AddForce(force * 200, ForceMode.Force);
                    rb.AddForceAtPosition(force * 50, this.transform.position + new Vector3(0.0f, 0.3f, 0.0f), ForceMode.Force);
                    convo.nextLines();
                    this.enabled = false;
                }
            }
        }
    }

    public void DeactivateObject()
    {
        active = false;
        selectable = false;
        Material m = GetComponent<MeshRenderer>().material;
        m.SetColor("_RimColor", new Vector4(0, 0, 0, 1));
        tkParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBubble>() && this.enabled)
        {//check if you touched the object WITH YOUR MIND
            selectable = true;
            tkParticle.Play();
            mat.SetColor("_RimColor", new Vector4(0.9680033f, 1, 0, 1));
        }
    }
}
