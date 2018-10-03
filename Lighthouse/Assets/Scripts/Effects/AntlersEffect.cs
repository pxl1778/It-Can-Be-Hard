using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AntlersEffect : MonoBehaviour {

    [SerializeField]
    private Transform playerHead;
    private Material mat;

    private void Start()
    {
        mat = this.transform.GetComponent<MeshRenderer>().sharedMaterial;
        GameManager.instance.EventMan.useBubble.AddListener(resetRadius);
    }

    private void OnDestroy()
    {
        GameManager.instance.EventMan.useBubble.RemoveListener(resetRadius);
    }

    // Update is called once per frame
    void Update () {
		if(playerHead != null)
        {
            mat.SetVector("_ObjectPoint", new Vector4(playerHead.position.x, playerHead.position.y + 0.25f, playerHead.position.z, 0));
        }
    }

    void resetRadius()
    {
        Debug.Log("resetting radius");
        StartCoroutine(iterateRadius());
    }

    IEnumerator iterateRadius()
    {
        mat.SetFloat("_Radius", 0);
        float radius = 0;
        while (radius < 1)
        {
            radius += Time.deltaTime * 1.5f;
            mat.SetFloat("_Radius", radius);
            yield return new WaitForEndOfFrame();
        }
    }
}
