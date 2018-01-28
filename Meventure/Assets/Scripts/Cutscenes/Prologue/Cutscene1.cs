using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene1 : Cutscene
{
    protected override void StartCutscene()
    {
        base.StartCutscene();
    } 

    protected override void PlayCutscene()
    {
        //foreach (KeyValuePair<string, CutsceneObject[]> pair in objectDictionary)
        //{
        //    //START LERPS
        //    pair.Value[0];
        //}
        gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][0].transform.position, objectDictionary["PlayerNodes"][0].running);
        //camera lerping
        //characters moving
        //graphical changes
        //type of lerp, length of time of lerp, thing to happen after lerp is done
    }

}
