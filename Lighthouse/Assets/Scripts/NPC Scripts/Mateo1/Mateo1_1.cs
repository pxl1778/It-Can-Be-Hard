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
        lines = new DialogueLine[] { new DialogueLine("mateo1_1_1")};
        options = new string[] { gm.DialogueMan.getLine("mateo1_1_option1"), gm.DialogueMan.getLine("mateo1_1_option2") };
        originalLines = lines;
        originalOptions = options;
        leaveCutscene = GameObject.Find("Neighbor1LeaveCutsceneCam1").GetComponent<Cutscene>();
    }

    public override void topOption()
    {
        base.topOption();
        switch (optionNumber)
        {
            case 0:
                lines = new DialogueLine[] { new DialogueLine("mateo1_1_2", () => { gm.EventMan.lerpToTarget.Invoke("shovel1", -45f, 1.0f); }, null, () => { gm.EventMan.lookAtPlayer.Invoke(); lines = new DialogueLine[] { new DialogueLine("mateo1_1_pretextbook") }; }) };
                options = new string[] { };
                lines[currentText].doLineStart();
                break;
            case 1:
                lines = new DialogueLine[] { new DialogueLine("mateo1_1_helped3", () => { }, null, () => { }) };
                options = new string[] { };
                lines[currentText].doLineStart();
                break;
            default:
                break;
        }
    }

    public override void bottomOption()
    {
        base.bottomOption();
        switch (optionNumber)
        {
            case 0:
                lines = new DialogueLine[] { new DialogueLine("mateo1_1_3", () => { gm.EventMan.lerpToTarget.Invoke("shovel1", -45f, 1.0f); }, null, () => { gm.EventMan.lookAtPlayer.Invoke(); lines = new DialogueLine[] { new DialogueLine("mateo1_1_pretextbook") }; }) };
                options = new string[] { };
                lines[currentText].doLineStart();
                break;
            case 1:
                lines = new DialogueLine[] { new DialogueLine("mateo1_1_helped3", () => { }, null, () => {  }) };
                options = new string[] { };
                lines[currentText].doLineStart();
                break;
            default:
                break;
        }
    }

    protected override void TalkedTo()
    {
        base.TalkedTo();
        if (!spokenTo)
        {
            //GameManager.instance.Globals.incrementTalkCount("mateo1Count");
            spokenTo = true;
        }
    }

    public override void checkWhenActivated()
    {
        if (gm.InventoryMan.HasItem("textbook"))
        {
            optionNumber++;
            lines = new DialogueLine[] { new DialogueLine("mateo1_1_helped1", null, ()=> { }), new DialogueLine("mateo1_1_helped2", null, () => { }) };
            options = new string[] { gm.DialogueMan.getLine("mateo1_1_helped_option1"), gm.DialogueMan.getLine("mateo1_1_helped_option2")};
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
