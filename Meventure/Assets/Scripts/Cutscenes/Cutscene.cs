using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour {

    public delegate void FinishedLerp();
    protected GameManager gm;
    protected Dictionary<string, CutsceneObject[]> objectDictionary = new Dictionary<string, CutsceneObject[]>();
    protected Dictionary<string, int> indexDictionary = new Dictionary<string, int>();
    protected Dictionary<string, List<FinishedLerp>> callbackDictionary = new Dictionary<string, List<FinishedLerp>>();

    // Use this for initialization
    void Start ()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        foreach(Transform t in this.transform)
        {
            objectDictionary.Add(t.name, t.GetComponentsInChildren<CutsceneObject>());
            indexDictionary.Add(t.name, 0);
            callbackDictionary.Add(t.name, new List<FinishedLerp>());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LerpCallback(string pName)
    {
        callbackDictionary[pName][indexDictionary[pName]].Invoke();
        indexDictionary[pName]++;
        //TODO: START NEXT OBJECT
    }

    protected virtual void StartCutscene()
    {
        gm.PlayerInfo.State = PlayerState.INACTIVE;
        gm.EventMan.stopPlayer.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCutscene();
        }
    }

    protected virtual void PlayCutscene()
    {
    }
}
