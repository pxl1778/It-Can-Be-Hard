using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public bool paused = false;
    private Canvas pauseMenu;
    private RawImage phoneImage;
    private float elapsedTime;
    private float lowerY = -1500.0f;
    private Coroutine lastRoutine = null;

    private void Start()
    {
        if (GameObject.Find("Pause Menu") != null)
        {
            pauseMenu = GameObject.Find("Pause Menu").GetComponent<Canvas>();
            phoneImage = pauseMenu.GetComponentInChildren<RawImage>();
            foreach (Transform t in pauseMenu.transform)
            {
                if(t.GetComponent<Image>() != null)
                {
                    t.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
                    if (t.childCount > 0)
                    {
                        t.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if(pauseMenu == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (lastRoutine != null)
            {
                StopCoroutine(lastRoutine);
            }
            if (paused)
            {
                Time.timeScale = 1;
                paused = false;
                lastRoutine = StartCoroutine(LerpPhoneDown(0.5f, lowerY));
                foreach (Transform t in pauseMenu.transform)
                {
                    if (t.GetComponent<Image>() != null)
                    {
                        t.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.2f, true);
                        if (t.childCount > 0)
                        {
                            t.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.2f, true);
                        }
                    }
                }
            }
            else
            {
                paused = true;
                lastRoutine = StartCoroutine(LerpPhone(0.5f, 0.0f));
                foreach (Transform t in pauseMenu.transform)
                {
                    if (t.GetComponent<Image>() != null)
                    {
                        t.GetComponent<Image>().CrossFadeAlpha(1.0f, 0.5f, true);
                        if (t.childCount > 0)
                        {
                            t.GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, 0.5f, true);
                        }
                    }
                }
            }
        }
    }

    IEnumerator LerpPhone(float duration, float endY)
    {
        elapsedTime = 0;
        float originalY = phoneImage.rectTransform.localPosition.y;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, Mathf.Lerp(originalY, endY, calcEase(elapsedTime / duration)), phoneImage.rectTransform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }
        phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, endY, phoneImage.rectTransform.localPosition.z);
        if(calcEase(elapsedTime / duration) >= 1.0f)
        {
            Time.timeScale = 0;
        }
    }
    IEnumerator LerpPhoneDown(float duration, float endY)
    {
        elapsedTime = 0;
        float originalY = phoneImage.rectTransform.localPosition.y;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, Mathf.Lerp(originalY, endY, calcEase(elapsedTime / duration)), phoneImage.rectTransform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }
        phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, endY, phoneImage.rectTransform.localPosition.z);
        pauseMenu.GetComponent<PauseMenuScript>().BackHome();
    }

    public float calcEase(float pAlpha)
    {
        pAlpha = (pAlpha > 1.0f) ? 1.0f : pAlpha;
        //return pAlpha * pAlpha * (3.0f - 2.0f * pAlpha);
        return pAlpha * (2.0f - pAlpha);
    }

    public void StartGame()
    {
        string currentScene = GameManager.instance.Load();
        if (currentScene != "")
        {
            GameManager.instance.StartLoadScene(currentScene);
        }
        else
        {
            GameManager.instance.StartLoadScene("PhoneTransition1");
        }
    }

    public void ExitGame()
    {
        Debug.Log("here");
        Time.timeScale = 1;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
