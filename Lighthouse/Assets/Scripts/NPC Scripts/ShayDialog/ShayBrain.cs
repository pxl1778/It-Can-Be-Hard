using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ShayBrain : MonoBehaviour
{

    private NPCTalkScript currentDialogue;
    [SerializeField]
    private bool sitting = false;
    [SerializeField]
    private bool normalScene = true;

    // Use this for initialization
    void Start()
    {
        this.transform.parent.GetComponentInChildren<Animator>().SetBool("Sitting", sitting);
        if (normalScene)
        {
            switch ((int)GameManager.instance.Globals.Dictionary["shayCount"])
            {
                case 0:
                    currentDialogue = this.gameObject.AddComponent<Shay_1>();
                    break;
                default:
                    currentDialogue = this.gameObject.AddComponent<Shay_1>();
                    break;
            }
        }
        else
        {
            //exceptions for cutscene dialogue
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
