using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene1 : Cutscene
{
    PathFollowingCharacter dog;

    protected override void StartCutscene()
    {
        base.StartCutscene();
        PlayCutscene();
    } 

    protected override void PlayCutscene()
    {
        //foreach (KeyValuePair<string, CutsceneObject[]> pair in objectDictionary)
        //{
        //    //START LERPS
        //    pair.Value[0];
        //}
        dog = GameObject.Find("Dog").GetComponent<PathFollowingCharacter>();
        dog.MoveToFinalPosition();
        gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][0].transform.position, objectDictionary["PlayerNodes"][0].running);
        gm.EventMan.lerpCameraToTransform.Invoke(objectDictionary["CameraNodes"][0].transform, objectDictionary["CameraNodes"][0].delay, objectDictionary["CameraNodes"][0].duration);

        callbackDictionary["PlayerNodes"][1] = () => {
            dog.StartMoveToPosition(objectDictionary["DogNodes"][0].transform.position, objectDictionary["DogNodes"][0].delay, objectDictionary["DogNodes"][0].duration);
        };
        callbackDictionary["DogNodes"][0] = () => {
            ParticleSystem.EmissionModule em = dog.GetComponentInChildren<ParticleSystem>().emission;
            em.rateOverTime = 250.0f;
            ParticleSystem.MainModule mm = dog.GetComponentInChildren<ParticleSystem>().main;
            mm.startSpeed = 3.0f;
        };
        callbackDictionary["DogNodes"][1] = () => {
            
        };
        //graphical changes
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
        if (pName == "DogNodes")
        {
            if (objectDictionary[pName].Length > indexDictionary[pName])
            {
                Debug.Log("Dog lerpcallback");
                dog.StartMoveToPosition(objectDictionary["DogNodes"][indexDictionary[pName]].transform.position, objectDictionary["DogNodes"][indexDictionary[pName]].delay, objectDictionary["DogNodes"][indexDictionary[pName]].duration);
            }
        }
    }

}
