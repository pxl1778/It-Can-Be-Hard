using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialCanvasScript : MonoBehaviour {

    private int currentIndex = -1;
    private bool tutorialOpen = false;

	// Use this for initialization
	void Start () {
        foreach(Transform t in transform)
        {
            if(t.GetComponent<Image>() != null)
            {
                t.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
                t.GetComponentsInChildren<Image>()[1].CrossFadeAlpha(0.0f, 0.0f, false);
                t.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentIndex)
        {
            case 0:
                if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
                {
                    FadeOutTutorial(1);
                }
                break;
            case 1:
                if (Input.GetMouseButton(0))
                {
                    FadeOutTutorial();
                    GameManager.instance.EventMan.startPlayerDialogue.Invoke(new string[] { GameManager.instance.DialogueMan.getLine("player_prologue_1") });
                }
                break;
            case 2:
                if (Input.GetButtonDown("Fire3"))
                {
                    FadeOutTutorial(3);
                }
                break;
            default:
                break;
        }
	}

    /// <summary>
    /// Fades in the next tutorial panel
    /// </summary>
    /// <param name="index">Number of tutorial panel to fade in.</param>
    public void FadeInTutorial(int index)
    {
        if (tutorialOpen)
        {
            FadeOutTutorial(index);
        }
        else
        {
            currentIndex = index;
            transform.GetChild(index).GetComponent<Image>().CrossFadeAlpha(1.0f, 0.5f, false);
            transform.GetChild(index).GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, 0.5f, false);
            transform.GetChild(index).GetComponentsInChildren<Image>()[1].CrossFadeAlpha(1.0f, 0.5f, false);
            tutorialOpen = true;
        }
    }
    
    /// <summary>
    /// Fades out one of the tutorial windows
    /// </summary>
    /// <param name="index">window to fade out</param>
    /// <param name="nextIndex"></param>
    public void FadeOutTutorial(int nextIndex = -1)
    {
        transform.GetChild(currentIndex).GetComponent<Image>().CrossFadeAlpha(0.0f, 0.3f, false);
        transform.GetChild(currentIndex).GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.3f, false);
        transform.GetChild(currentIndex).GetComponentsInChildren<Image>()[1].CrossFadeAlpha(0.0f, 0.3f, false);
        tutorialOpen = false;
        currentIndex = -1;
        if(nextIndex > -1)
        {
            StartCoroutine(WaitForFade(1.0f, nextIndex));
        }
    }

    IEnumerator WaitForFade(float duration, int nextIndex)
    {
        float elapsedTime = 0.0f;
        float alpha = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        FadeInTutorial(nextIndex);
    }
}
