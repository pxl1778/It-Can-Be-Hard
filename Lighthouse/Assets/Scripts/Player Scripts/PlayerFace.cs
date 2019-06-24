using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFace : MonoBehaviour {

    private Material[] mat;
    private SkinnedMeshRenderer faceRenderer;
    private MaterialPropertyBlock propBlock;
    [SerializeField]
    private float faceNum = 0.0f;

    // Use this for initialization
    void Start () {
        faceRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        GameManager.instance.EventMan.changePlayerFace.AddListener(changePlayerFace);
        mat = faceRenderer.materials;
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0; i<5; i++)
        {
            if(mat.Length > i)
            {
                mat[i].SetFloat("_FaceNumber", faceNum);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            faceNum = 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            faceNum = 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            faceNum = 0.0f;
        }
	}

    void changePlayerFace(float offset)
    {
        faceNum = offset;
        for (int i = 0; i < 5; i++)
        {
            mat[i].SetFloat("_FaceNumber", faceNum);
        }
    }
}
