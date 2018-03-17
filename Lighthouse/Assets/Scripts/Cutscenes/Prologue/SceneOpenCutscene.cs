using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOpenCutscene : Cutscene {

    protected override void StartCutscene()
    {
        base.StartCutscene();
        PlayCutscene();
    }

    protected override void PlayCutscene()
    {
        gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][0].transform.position, objectDictionary["PlayerNodes"][0].running);
        gm.EventMan.lerpCameraToTransform.Invoke(objectDictionary["CameraNodes"][0].transform, objectDictionary["CameraNodes"][0].delay, objectDictionary["CameraNodes"][0].duration);
    }

    public override void LerpCallback(string pName)
    {
        base.LerpCallback(pName);

        if (pName == "PlayerNodes")
        {
            if (objectDictionary[pName].Length > indexDictionary[pName])
            {
                gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][indexDictionary[pName]].transform.position, objectDictionary["PlayerNodes"][indexDictionary[pName]].running);
            }
        }
        if (pName == "CameraNodes")
        {
            if (objectDictionary[pName].Length > indexDictionary[pName])
            {
                gm.EventMan.lerpCameraToTransform.Invoke(objectDictionary["CameraNodes"][indexDictionary[pName]].transform, objectDictionary["CameraNodes"][indexDictionary[pName]].delay, objectDictionary["CameraNodes"][indexDictionary[pName]].duration);
            }
        }
    }

    protected override void EndCutscene()
    {
        base.EndCutscene();
        gm.EventMan.startPlayerDialogue.Invoke(new string[] { gm.DialogueMan.getLine("player_prologue_1") });
    }
}
