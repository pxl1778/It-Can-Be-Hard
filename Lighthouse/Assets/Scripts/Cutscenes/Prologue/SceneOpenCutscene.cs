using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOpenCutscene : Cutscene {

    Cinemachine.CinemachineVirtualCamera cutsceneCam;

    protected override void StartCutscene()
    {
        base.StartCutscene();
        cutsceneCam = this.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        cutsceneCam.Priority = 11;
        GameManager.instance.EventMan.endCutscene.AddListener(EndCutscene);
        cameraTimeline.Play();
        PlayCutscene();
    }

    protected override void PlayCutscene()
    {
        //gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][0].transform.position, objectDictionary["PlayerNodes"][0].delay, objectDictionary["PlayerNodes"][0].running);
        //gm.EventMan.lerpCameraToTransform.Invoke(objectDictionary["CameraNodes"][0].transform, objectDictionary["CameraNodes"][0].delay, objectDictionary["CameraNodes"][0].duration);
    }

    public override void LerpCallback(string pName)
    {
        base.LerpCallback(pName);

        if (pName == "PlayerNodes")
        {
            if (objectDictionary[pName].Length > indexDictionary[pName])
            {
                gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][indexDictionary[pName]].transform.position, objectDictionary["PlayerNodes"][indexDictionary[pName]].delay, objectDictionary["PlayerNodes"][indexDictionary[pName]].running);
            }
        }
    }

    protected override void EndCutscene()
    {
        base.EndCutscene();
        GameManager.instance.EventMan.endCutscene.RemoveListener(EndCutscene);
        gm.EventMan.startPlayerDialogue.Invoke(new string[] { gm.DialogueMan.getLine("player_prologue_1") });
        cutsceneCam.Priority = 0;
    }
}
