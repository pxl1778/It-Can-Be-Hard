using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class BoulderPush : MonoBehaviour {

    private float alpha = 0;
    private bool selectable = false;
    private bool active = false;
    private PlayableDirector director;
    private ParticleSystem tkParticle;
    private Material mat;
    //Cinemachine.CinemachineFreeLook freeLook;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera boulderCam;

    private void Start()
    {
        director = transform.GetComponent<PlayableDirector>();
        tkParticle = transform.GetComponentInChildren<ParticleSystem>();
        mat = GetComponentInChildren<MeshRenderer>().material;
    }

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
                    GameManager.instance.Player.State = PlayerState.INACTIVE;
                    //move camera priority
                    //freeLook = (Cinemachine.CinemachineFreeLook)Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera;
                    //freeLook.LookAt = this.transform;
                    boulderCam.Priority = 100;
                    selectable = false;
                    active = true;
                }
            }
        }
        else if (active && Input.GetMouseButtonDown(0))
        {
            active = false;
            tkParticle.Stop();
            mat.SetColor("_RimColor", new Vector4(0, 0, 0, 1));
            director.Play();
        }
    }

    public void DeactivateObject()
    {
        active = false;
        selectable = false;
        Material m = GetComponent<MeshRenderer>().material;
        m.SetColor("_RimColor", new Vector4(0, 0, 0, 1));
        tkParticle.Stop();
        boulderCam.Priority = 0;
        GameManager.instance.Player.State = PlayerState.ACTIVE;
        GameObject.Find("TutorialCanvas").GetComponent<TutorialCanvasScript>().FadeOutTutorial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBubble>())
        {//check if you touched the object WITH YOUR MIND
            selectable = true;
            tkParticle.Play();
            mat.SetColor("_RimColor", new Vector4(0.9680033f, 1, 0, 1));
        }
    }
}
