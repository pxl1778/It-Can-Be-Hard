using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Mateo1_1 : NPCTalkScript {

    private bool spokenTo = false;

    public override void secondStart()
    {
        lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("mateo1_1_1"))
                                    };
        options = new string[] { gm.DialogueMan.getLine("mateo1_1_option1"), gm.DialogueMan.getLine("mateo1_1_option2") };
        originalLines = lines;
        originalOptions = options;
    }

    public override void topOption()
    {
        base.topOption();
        switch (optionNumber)
        {
            case 0:
                lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("mateo1_1_2"), () => { gm.EventMan.lerpToTarget.Invoke("shovel1", -45f, 1.0f); }, null, () => { gm.EventMan.lookAtPlayer.Invoke(); lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("mateo1_1_preshovel")) }; }) };
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
                lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("mateo1_1_3"), () => { gm.EventMan.lerpToTarget.Invoke("shovel1", -45f, 1.0f); }, null, () => { gm.EventMan.lookAtPlayer.Invoke(); lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("mateo1_1_pretextbook")) }; }) };
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
        if (gm.InventoryMan.HasItem("textbook"))
        {
            lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("mateo1_1_helped"), null, ()=> { GameObject.Destroy(GameObject.Find("stump")); }) };
            options = new string[] { };
        }
    }
}
