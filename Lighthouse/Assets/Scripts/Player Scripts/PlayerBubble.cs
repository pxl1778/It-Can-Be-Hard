using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBubble : MonoBehaviour {

    private bool active;
    private Vector3 startScale = new Vector3(0.01f, 0.01f, 0.01f);
    [SerializeField]
    private Vector3 endScale = new Vector3(0.7f, 0.7f, 0.7f);
    private float alpha = 0;
    private float speed = 1;
    private MeshRenderer meshRenderer;
    private AudioSource bubbleSound;

	// Use this for initialization
	void Start () {
        meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        Deactivate();
        bubbleSound = this.GetComponentInChildren<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            alpha += speed * Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(startScale, endScale, alpha);
            if(alpha >= .8f)
            {
                Color c = meshRenderer.material.color;
                c.a = Mathf.Lerp(.2f, 0, (alpha - .8f) * (1/.2f));
                meshRenderer.material.SetColor("_Color", c);
            }
            if(alpha >= 1)
            {
                if(meshRenderer != null)
                {
                    Deactivate();
                }
            }
        }
	}

    public void Activate()
    {
        Debug.Log("bubble activate");
        active = true;
        this.transform.localScale = startScale;
        alpha = 0;
        meshRenderer.enabled = true;
        Color c = meshRenderer.material.color;
        c.a = .2f;
        meshRenderer.material.SetColor("_Color", c);
        bubbleSound.pitch = Random.Range(0.85f, 1.1f);
        bubbleSound.volume = 1.0f;
        bubbleSound.PlayOneShot(bubbleSound.clip);
    }

    public void Deactivate()
    {
        Debug.Log("deactivating");
        if (meshRenderer != null)
        {
            Debug.Log("deactivating successfully");
            active = false;
            alpha = 0;
            this.transform.localScale = startScale;
            this.enabled = false;
            meshRenderer.enabled = false;
        }
    }
}
