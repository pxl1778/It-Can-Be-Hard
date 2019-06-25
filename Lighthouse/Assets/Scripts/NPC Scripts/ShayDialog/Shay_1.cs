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
        manageDictionary(new string[]{ "Shay_1"});
        //lines = new DialogueLine[] { new DialogueLine("shay_1_1") };
        //options = new string[] { };
        originalLines = lines;
        originalOptions = options;
        //leaveCutscene = GameObject.Find("Neighbor1LeaveCutsceneCam1").GetComponent<Cutscene>();
    }

    protected override void TalkedTo()
    {
        base.TalkedTo();
        if (!spokenTo)
        {
            GameManager.instance.Globals.IncrementTalkCount(Character.SHAY);
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
