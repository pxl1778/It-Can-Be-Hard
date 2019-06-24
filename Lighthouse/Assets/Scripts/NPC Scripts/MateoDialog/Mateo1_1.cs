using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Mateo1_1 : NPCTalkScript {

    private bool spokenTo = false;
    private Cutscene leaveCutscene;

    public override void secondStart()
    {
        manageDictionary(new string[] { "Mateo1_1", "Mateo1_1_pretextbook", "Mateo1_1_helped" });
        dialogueDict["mateo1_1_2"].lineComplete = () => { gm.EventMan.lookAtPlayer.Invoke(); setupLines(dialogueDict["mateo1_1_pretextbook"]); };
        dialogueDict["mateo1_1_3"].lineComplete = () => { gm.EventMan.lookAtPlayer.Invoke(); setupLines(dialogueDict["mateo1_1_pretextbook"]); };
        originalLines = lines;
        originalOptions = options;
        leaveCutscene = GameObject.Find("Neighbor1LeaveCutsceneCam1").GetComponent<Cutscene>();
    }

    protected override void TalkedTo()
    {
        base.TalkedTo();
        if (!spokenTo)
        {
            spokenTo = true;
        }
    }

    public override void checkWhenActivated()
    {
        if (gm.InventoryMan.HasItem("textbook"))
        {
            Debug.Log("check when activated and has textbook");
            optionNumber = 0;
            setupLines("Mateo1_1_helped");
        }
    }

    protected override void exitDialogue()
    {
        base.exitDialogue();
        if (gm.InventoryMan.HasItem("textbook"))
        {
            this.GetComponent<Collider>().enabled = false;
            active = false;
            speechBubble.GetComponent<MeshRenderer>().enabled = false;
            leaveCutscene.StartCutscene();
            this.transform.parent.GetComponentInChildren<Animator>().SetBool("Turning", true);
        }
    }
}
