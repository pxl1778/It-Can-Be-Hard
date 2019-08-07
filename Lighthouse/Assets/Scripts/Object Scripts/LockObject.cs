using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LockObject : StaticObjects {

    [SerializeField]
    protected GameObject gate;

    override protected void Interaction()
    {
        selectable = false;
        active = false;
        tkParticle.Stop();
        mat.SetColor("_EffectColor", Color.black);
        hitParticles.Play();
        this.gameObject.transform.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine("LerpGate");
    }

    protected override void ActivateObject()
    {
        if(SceneManager.GetActiveScene().name == "Neighborhood1")
        {
            selectable = true;
            tkParticle.Play();
            mat.SetColor("_EffectColor", originalColor);
        }
    }

    IEnumerator LerpGate()
    {
        float elapsedTime = 0.0f;
        float alpha = 0;
        float duration = 1.0f;
        Quaternion startRot = gate.transform.rotation;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            alpha = elapsedTime / duration;
            if(alpha > 1.0f)
            {
                alpha = 1.0f;
            }
            gate.transform.rotation = startRot * Quaternion.AngleAxis(alpha * 80.0f, Vector3.up);
            yield return new WaitForEndOfFrame();
        }
        //gate.transform.rotation.y = endRot;
        GameObject.Destroy(this.gameObject);
    }
}
