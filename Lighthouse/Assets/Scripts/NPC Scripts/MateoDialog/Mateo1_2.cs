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

        manageDictionary(new string[] { "Mateo1_2"});
        originalLines = lines;
        originalOptions = options;
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
            GameManager.instance.Globals.IncrementTalkCount(Character.MATEO);
            spokenTo = true;
        }
    }

    public override void checkWhenActivated()
    {
    }
}
