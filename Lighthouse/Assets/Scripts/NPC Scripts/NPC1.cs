using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NPC1 : NPCTalkScript {

    public override void secondStart()
    {
        //Debug.Log(gm.DialogueMan.getLine("neighbor1_1"));
        lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("neighbor1_1"))
                                    };
        options = new string[] { gm.DialogueMan.getLine("neighbor1_option1"), gm.DialogueMan.getLine("neighbor1_option2") };
        originalLines = lines;
        originalOptions = options;
    }

    public override void topOption()
    {
        switch (optionNumber)
        {
            case 0:
                lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("neighbor1_2"), () => { gm.EventMan.lerpToTarget.Invoke("shovel1", -45f, 1.0f); }, null, () => { gm.EventMan.lookAtPlayer.Invoke(); lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("neighbor1_preshovel")) }; }) };
                options = new string[] { };
                lines[currentText].doLineStart();
                break;
            default:
                break;
        }
    }

    public override void bottomOption()
    {
        switch (optionNumber)
        {
            case 0:
                lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("neighbor1_3"), () => { gm.EventMan.lerpToTarget.Invoke("shovel1", -45f, 1.0f); }, null, () => { gm.EventMan.lookAtPlayer.Invoke(); lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("neighbor1_preshovel")) }; }) };
                options = new string[] { };
                lines[currentText].doLineStart();
                break;
            default:
                break;
        }
    }

    public override void checkWhenActivated()
    {
        if (gm.InventoryMan.HasItem("shovel"))
        {
            lines = new DialogueLine[] { new DialogueLine(gm.DialogueMan.getLine("neighbor1_helped"), null, ()=> { GameObject.Destroy(GameObject.Find("stump")); }) };
            options = new string[] { };
        }
    }
}
