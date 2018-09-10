using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Mateo1Brain : MonoBehaviour {

    private NPCTalkScript currentDialogue;
    [SerializeField]
    private bool sitting = false;
    [SerializeField]
    private bool normalScene = true;

	// Use this for initialization
	void Start ()
    {
        this.transform.parent.GetComponentInChildren<Animator>().SetBool("Sitting", sitting);
        if(normalScene)
        {
            switch ((int)GameManager.instance.Globals.Dictionary["mateo1Count"])
            {
                case 0:
                    currentDialogue = this.gameObject.AddComponent<Mateo1_1>();
                    break;
                case 1:
                    currentDialogue = this.gameObject.AddComponent<Mateo1_2>();
                    break;
                case 2:
                    currentDialogue = this.gameObject.AddComponent<Mateo1_3>();
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Town1")
            {
                currentDialogue = this.gameObject.AddComponent<Mateo1_Town1>();
            }
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
