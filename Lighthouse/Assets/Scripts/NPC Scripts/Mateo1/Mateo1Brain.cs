using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mateo1Brain : MonoBehaviour {

    private NPCTalkScript currentDialogue;

	// Use this for initialization
	void Start () {
        switch ((int)GameManager.instance.Globals.Dictionary["mateo1Count"])
        {
            case 0:
                currentDialogue = this.gameObject.AddComponent<Mateo1_1>();
                break;
            case 1:
                currentDialogue =  this.gameObject.AddComponent<Mateo1_2>();
                break;
            case 2:
                currentDialogue =  this.gameObject.AddComponent<Mateo1_3>();
                break;
            default:
                break;
        }
	}
	
	public void topOption()
    {
        currentDialogue.topOption();
    }

    public void bottomOption()
    {
        currentDialogue.bottomOption();
    }
}
