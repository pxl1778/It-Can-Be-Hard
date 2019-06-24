using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mateo1_Town1Seated : NPCTalkScript
{

    private bool spokenTo = false;
    private Cutscene leaveCutscene;

    public override void secondStart()
    {
        lines = new DialogueLine[] { new DialogueLine("mateo1_1_1")
                                    };
        //options = new string[] { gm.DialogueMan.getLine("mateo1_1_option1"), gm.DialogueMan.getLine("mateo1_1_option2") };
        originalLines = lines;
        originalOptions = options;
        leaveCutscene = GameObject.Find("Neighbor1LeaveCutsceneCam1").GetComponent<Cutscene>();
    }

    public override void topOption()
    {
        base.topOption();
    }

    public override void bottomOption()
    {
        base.bottomOption();
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
    }

    protected override void exitDialogue()
    {
        base.exitDialogue();
    }
}
