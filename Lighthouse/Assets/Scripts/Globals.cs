using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {

    private Dictionary<string, object> dictionary;
    public Dictionary<string, object> Dictionary { get { return dictionary; } }
    public delegate void finishLerp();

    void Start () {
        //savedata
        GameManager.instance.EventMan.lerpGlobalValue.AddListener(StartGlobalLerp);
        dictionary = new Dictionary<string, object>();

        dictionary.Add("glowRadius", 0.0f);
	}

    public void StartGlobalLerp(string variableName, float endLerp, float duration)
    {
        StartCoroutine(LerpGlobal(variableName, endLerp, duration));
    }

    IEnumerator LerpGlobal(string lerpString, float endLerp, float duration)
    {
        float elapsedTime = 0.0f;
        float alpha = 0;
        //if(dictionary[lerpString])//check it's the right type
        float startLerp = (float)dictionary[lerpString];
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            alpha = elapsedTime / duration;
            dictionary[lerpString] = Mathf.Lerp(startLerp, endLerp, alpha);
            yield return new WaitForEndOfFrame();
        }
    }
}
