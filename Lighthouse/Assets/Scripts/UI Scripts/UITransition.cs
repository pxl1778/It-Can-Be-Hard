using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransition : MonoBehaviour {

    private Image blackScreen;
    private Canvas canvas;
    private float timeElapsed;

	void Start () {
        blackScreen = this.GetComponentInChildren<Image>();
        canvas = this.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartFadeOut(float duration = 1.0f)
    {
        blackScreen.color = new Color(0, 0, 0, 0);
        StartCoroutine(FadeOut(duration));
    }

    IEnumerator FadeOut(float duration)
    {
        float alpha = 0;
        timeElapsed = 0;
        while(timeElapsed <= duration)
        {
            timeElapsed += Time.deltaTime;
            alpha = timeElapsed / duration;
            alpha = (alpha > 1.0f) ? 1 : alpha;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
            yield return new WaitForEndOfFrame();
        }
        GameManager.instance.LoadScene();
    }

    public void StartFadeIn(float duration = 1.0f)
    {
        if(blackScreen == null)
        {
            blackScreen = this.GetComponentInChildren<Image>();
        }
        blackScreen.color = new Color(0, 0, 0, 1.0f);
        StartCoroutine(FadeIn(duration));
    }

    IEnumerator FadeIn(float duration)
    {
        float alpha = 1;
        timeElapsed = 0;
        while (timeElapsed <= duration)
        {
            timeElapsed += Time.deltaTime;
            alpha = 1 - (timeElapsed / duration);
            alpha = (alpha < 0.0f) ? 0 : alpha;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
            yield return new WaitForEndOfFrame();
        }
        if (GameManager.instance.Player != null) { GameManager.instance.Player.State = PlayerState.ACTIVE; }
        canvas.enabled = false;
    }
}
