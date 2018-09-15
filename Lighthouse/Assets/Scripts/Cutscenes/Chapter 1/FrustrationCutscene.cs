using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FrustrationCutscene : Cutscene {
    
    [SerializeField]
    private Transform playerStartPos;
    [SerializeField]
    private PlayerBubble bubble;
    [SerializeField]
    private TutorialCanvasScript tutorialCanvasScript;

    public override void StartCutscene()
    {
        gm.Player.State = PlayerState.CUTSCENE;
        gm.EventMan.stopPlayer.Invoke();
        cameraTimeline = this.GetComponent<PlayableDirector>();
        cameraTimeline.Play();
        StartCoroutine(WaitForEnd(cameraTimeline.playableAsset.duration));
        PlayCutscene();
        GameManager.instance.EventMan.changePlayerFace.Invoke(0.1f);
        StartCoroutine(WaitAFew(2.0f));
        StartCoroutine(WaitTilBubble(16.45f));
    }

    protected override void PlayCutscene()
    {}

    protected override void EndCutscene()
    {
        gm.Player.State = PlayerState.ACTIVE;
        tutorialCanvasScript.FadeInTutorial(3);
        GameManager.instance.EventMan.changePlayerFace.Invoke(0.0f);
        GameManager.instance.EventMan.startPlayerDialogue.Invoke(new string[] { GameManager.instance.DialogueMan.getLine("player_chapter1_1") });
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

    IEnumerator WaitAFew(double duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        GameManager.instance.Player.transform.position = new Vector3(playerStartPos.position.x, GameManager.instance.Player.transform.position.y, playerStartPos.position.z);
        GameManager.instance.Player.transform.rotation = playerStartPos.rotation;
    }

    IEnumerator WaitTilBubble(double duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        bubble.enabled = true;
        bubble.Activate();
        GameManager.instance.EventMan.changePlayerFace.Invoke(0.2f);
    }
}
