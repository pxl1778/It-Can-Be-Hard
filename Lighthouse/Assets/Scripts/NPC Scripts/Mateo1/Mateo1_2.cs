using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Mateo1_2 : NPCTalkScript
{

    private bool spokenTo = false;

    public override void secondStart()
    {
        lines = new DialogueLine[] { new DialogueLine("mateo1_2_1")
                                    };
        options = new string[] { gm.DialogueMan.getLine("mateo1_2_option1"), gm.DialogueMan.getLine("mateo1_2_option2") };
        originalLines = lines;
        originalOptions = options;
    }

    public override void topOption()
    {
        base.topOption();
        switch (optionNumber)
        {
            case 0:
                lines = new DialogueLine[] { new DialogueLine("mateo1_2_2") };
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
                lines = new DialogueLine[] { new DialogueLine("mateo1_2_3") };
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
}
