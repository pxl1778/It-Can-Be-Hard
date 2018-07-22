using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrassRedoScript : MonoBehaviour
{

    private GameObject player;
    private GameObject glow;
    private Material mat;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        glow = GameObject.Find("Dog");
        mat = this.transform.GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            mat.SetVector("_ObjectPoint", new Vector4(player.transform.position.x, player.transform.position.y + 0.25f, player.transform.position.z, 0));
        }
        //if (glow != null)
        //{
        //    mat.SetVector("_GlowObjectPoint", new Vector4(glow.transform.position.x, glow.transform.position.y, glow.transform.position.z, 0));
        //}
        //else
        //{
        //    mat.SetVector("_GlowObjectPoint", new Vector4(0, -500, 0, 0));
        //}
    }
}
