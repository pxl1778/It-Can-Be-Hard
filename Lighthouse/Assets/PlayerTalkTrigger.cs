using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalkTrigger : MonoBehaviour {

    [SerializeField]
    private string dialogueLine;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("here");
        if(collision.transform.tag == "Player")
        {
            GameManager.instance.EventMan.startPlayerDialogue.Invoke(new string[] { GameManager.instance.DialogueMan.getLine(dialogueLine) });
        }
    }
}
