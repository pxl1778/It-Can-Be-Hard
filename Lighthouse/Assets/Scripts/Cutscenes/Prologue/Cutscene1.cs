using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene1 : Cutscene
{
    PathFollowingCharacter dog;
    

    public override void StartCutscene()
    {
        base.StartCutscene();
        this.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
        cameraTimeline.Play();
        ResetPlayerParent();
        GameManager.instance.EventMan.endCutscene.AddListener(EndCutscene);
        StartCoroutine(FadeOutAudio(0.5f, Camera.main.GetComponentInChildren<AudioSource>()));
        StartCoroutine(WaitForEnd(cameraTimeline.playableAsset.duration));
        PlayCutscene();
    } 

    protected override void PlayCutscene()
    {
        //foreach (KeyValuePair<string, CutsceneObject[]> pair in objectDictionary)
        //{
        //    //START LERPS
        //    pair.Value[0];
        //}
        //dog = GameObject.Find("Dog").GetComponent<PathFollowingCharacter>();
        //dog.MoveToFinalPosition();//handy for testing
        //gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][0].transform.position, objectDictionary["PlayerNodes"][0].delay, objectDictionary["PlayerNodes"][0].running);
        //gm.EventMan.lerpCameraToTransform.Invoke(objectDictionary["CameraNodes"][0].transform, objectDictionary["CameraNodes"][0].delay, objectDictionary["CameraNodes"][0].duration);

        callbackDictionary["PlayerNodes"][1] = () => {
            dog.StartMoveToPosition(objectDictionary["DogNodes"][0].transform.position, objectDictionary["DogNodes"][0].delay, objectDictionary["DogNodes"][0].duration);
        };
        callbackDictionary["DogNodes"][0] = () => {
            ParticleSystem.EmissionModule em = dog.GetComponentInChildren<ParticleSystem>().emission;
            em.rateOverTime = 250.0f;
            ParticleSystem.MainModule mm = dog.GetComponentInChildren<ParticleSystem>().main;
            mm.startSpeed = 3.0f;
            GameManager.instance.EventMan.lerpGlobalValue.Invoke(GlobalData.GLOW_RADIUS, 10, 1);
        };
        callbackDictionary["DogNodes"][1] = () => {
            
        };
    }

    public override void LerpCallback(string pName)
    {
        base.LerpCallback(pName);

        if (pName == "PlayerNodes")
        {
            if (objectDictionary[pName].Length > indexDictionary[pName])
            {
                //gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][indexDictionary[pName]].transform.position, objectDictionary["PlayerNodes"][indexDictionary[pName]].delay, objectDictionary["PlayerNodes"][indexDictionary[pName]].running);
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

    protected override void EndCutscene()
    {
        base.EndCutscene();
        cameraTimeline.Stop();
        GameManager.instance.StartLoadScene("PhoneTransition2");
    }

    IEnumerator WaitForEnd(double duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        EndCutscene();
    }

    IEnumerator FadeOutAudio(float duration, AudioSource audio)
    {
        float elapsedTime = 0.0f;
        float originalVolume = audio.volume;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(originalVolume, 0, elapsedTime / duration);
            yield return new WaitForEndOfFrame();
        }
        audio.volume = 0;
    }

}
