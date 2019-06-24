using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Mateo1_Town1 : NPCTalkScript
{

    private bool spokenTo = false;

    public override void secondStart()
    {
        manageDictionary(new string[] { "Mateo1_Town1" });
        dialogueDict["mateo1_Town1"].lineComplete = () => { GameManager.instance.StartLoadScene("Town1Seated"); };
        setupLines(dialogueDict["mateo1_Town1"]);
        originalLines = lines;
        originalOptions = options;
        turnTowardsPlayer = false;
        this.transform.parent.GetComponentInChildren<Animator>().SetBool("Sitting", true);
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
            //GameManager.instance.Globals.incrementTalkCount("mateo1Count");
            spokenTo = true;
        }
    }

    public override void checkWhenActivated()
    {
    }
}

