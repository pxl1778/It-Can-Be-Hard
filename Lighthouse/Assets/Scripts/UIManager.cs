﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

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

    public void ResetPauseMenu()
    {
        if (GameObject.Find("Pause Menu") != null)
        {
            pauseMenu = GameObject.Find("Pause Menu").GetComponent<Canvas>();
            phoneImage = pauseMenu.GetComponentInChildren<RawImage>();
            foreach (Transform t in pauseMenu.transform)
            {
                if (t.GetComponent<Image>() != null)
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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //delete later
            GameManager.instance.ClearSaveData();
        }
        if(pauseMenu == null)
        {
            return;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (lastRoutine != null)
            {
                Debug.Log("last routine isn't null");
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
                //disable camera controls
                GameManager.instance.Player.State = PlayerState.INACTIVE;
                GameManager.instance.Player.DeactivateCamera();
                pauseMenu.GetComponentInChildren<Image>().enabled = true;
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
        pauseMenu.GetComponent<PauseMenuScript>().PlayBackBlip();
        elapsedTime = 0;
        float originalY = phoneImage.rectTransform.localPosition.y;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, Mathf.Lerp(originalY, endY, calcEase(elapsedTime / duration)), phoneImage.rectTransform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }
        phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, endY, phoneImage.rectTransform.localPosition.z);
        pauseMenu.GetComponent<PauseMenuScript>().ResetPhone();
        pauseMenu.GetComponentInChildren<Image>().enabled = false;
        GameManager.instance.Player.State = PlayerState.ACTIVE;
        GameManager.instance.Player.ActivateCamera();
        GameManager.instance.Save();
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
            //Load scene from save data
            GameManager.instance.StartLoadScene(currentScene);
        }
        else
        {
            //No data, new game
            GameManager.instance.StartLoadScene("PhoneTransition1");
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
