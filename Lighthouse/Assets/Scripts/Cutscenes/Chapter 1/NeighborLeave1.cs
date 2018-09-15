using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborLeave1 : Cutscene
{
    Cinemachine.CinemachineVirtualCamera cutsceneCam;
    [SerializeField]
    private Transform NPC;

    public override void StartCutscene()
    {
        gm.Player.State = PlayerState.CUTSCENE;
        gm.EventMan.stopPlayer.Invoke();
        cutsceneCam = this.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        cameraTimeline.Play();
        StartCoroutine(WaitForEnd(cameraTimeline.playableAsset.duration));
        if (NPC.GetComponentInChildren<Collider>().isTrigger)
        {
            NPC.GetComponentInChildren<Collider>().enabled = false;
            NPC.GetComponentInChildren<NPCTalkScript>().enabled = false;
        }
        PlayCutscene();
    }

    protected override void PlayCutscene()
    {
        //gm.EventMan.movePlayerToPosition.Invoke(objectDictionary["PlayerNodes"][0].transform.position, objectDictionary["PlayerNodes"][0].delay, objectDictionary["PlayerNodes"][0].running);
        //gm.EventMan.lerpCameraToTransform.Invoke(objectDictionary["CameraNodes"][0].transform, objectDictionary["CameraNodes"][0].delay, objectDictionary["CameraNodes"][0].duration);
    }

    protected override void EndCutscene()
    {
        gm.Player.State = PlayerState.ACTIVE;
        NPC.gameObject.SetActive(false);
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
}
