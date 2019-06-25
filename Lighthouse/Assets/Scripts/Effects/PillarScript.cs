using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour {

    private Material mat;
    private GameObject glow;
    private GameManager gm;
    private MeshRenderer mr;

	// Use this for initialization
	void Start () {
        mat = this.GetComponent<MeshRenderer>().material;
        glow = GameObject.Find("Dog");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        mr = this.GetComponent<MeshRenderer>();
        mr.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        mat.SetVector("_MaterializingPoint", new Vector4(glow.transform.position.x, glow.transform.position.y, glow.transform.position.z, 0));
        mat.SetFloat("_Radius", (float)gm.Globals.Dictionary[GlobalData.GLOW_RADIUS]);
    }
}
