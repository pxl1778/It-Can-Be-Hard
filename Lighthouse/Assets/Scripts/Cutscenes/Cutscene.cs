using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour {

    public delegate void FinishedLerp();
    protected GameManager gm;
    protected Dictionary<string, CutsceneObject[]> objectDictionary = new Dictionary<string, CutsceneObject[]>();
    protected Dictionary<string, int> indexDictionary = new Dictionary<string, int>();
    protected Dictionary<string, FinishedLerp[]> callbackDictionary = new Dictionary<string, FinishedLerp[]>();
    private bool playing = false;
    private int amount = 0;

    // Use this for initialization
    void Start ()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        foreach(Transform t in this.transform)
        {
            objectDictionary.Add(t.name, t.GetComponentsInChildren<CutsceneObject>());//nodes that will be used with cutscene info
            indexDictionary.Add(t.name, 0);//which node is currently happening
            callbackDictionary.Add(t.name, new FinishedLerp[objectDictionary[t.name].Length]);//array of callback functions for each cutscene object
            amount += objectDictionary[t.name].Length;
        }
    }

    public virtual void LerpCallback(string pName)
    {
        if (objectDictionary.ContainsKey(pName) && objectDictionary[pName].Length > indexDictionary[pName])
        {
            Debug.Log(pName + " " + objectDictionary[pName].Length);
            if (callbackDictionary[pName][indexDictionary[pName]] != null)//if there's a callback function, invoke it
            {
                callbackDictionary[pName][indexDictionary[pName]].Invoke();
            }
            indexDictionary[pName]++;//go onto next event
            amount--;
            if(amount <= 0)
            {
                EndCutscene();
            }
        }
    }

    protected virtual void StartCutscene()
    {
        gm.EventMan.finishedLerp.AddListener(LerpCallback);
        gm.Player.State = PlayerState.INACTIVE;
        gm.EventMan.stopPlayer.Invoke();
    }

    protected virtual void EndCutscene()
    {
        gm.EventMan.lookAtPlayer.Invoke();
        gm.Player.State = PlayerState.ACTIVE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playing)
        {

            if (other.gameObject.tag == "Player")
            {
                StartCutscene();
                playing = true;
            }
        }
    }

    protected virtual void PlayCutscene() {}
}
