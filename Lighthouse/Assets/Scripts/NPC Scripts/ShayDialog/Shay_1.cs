using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Shay_1 : NPCTalkScript
{

    private bool spokenTo = false;
    private Cutscene leaveCutscene;

    public override void secondStart()
    {
        lines = new DialogueLine[] { new DialogueLine("shay_1_1") };
        options = new string[] { };
        originalLines = lines;
        originalOptions = options;
        //leaveCutscene = GameObject.Find("Neighbor1LeaveCutsceneCam1").GetComponent<Cutscene>();
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
                lines = new DialogueLine[] { new DialogueLine("mateo1_1_helped3", () => { }, null, () => { }) };
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
            GameManager.instance.Globals.incrementTalkCount("mateo1Count");
            spokenTo = true;
        }
    }

    public override void checkWhenActivated()
    {
        
    }

    protected override void exitDialogue()
    {
        base.exitDialogue();
    }
}
