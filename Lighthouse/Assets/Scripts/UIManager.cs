using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public bool paused = false;
    private Canvas pauseMenu;

    private void Start()
    {
        if (GameObject.Find("Pause Menu") != null)
        {
            pauseMenu = GameObject.Find("Pause Menu").GetComponent<Canvas>();
            foreach (Transform t in pauseMenu.transform)
            {
                t.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
                if (t.childCount > 0)
                {
                    t.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
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
            Debug.Log("update escape");
            if (paused)
            {
                Time.timeScale = 1;
                paused = false;
                foreach (Transform t in pauseMenu.transform)
                {
                    t.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.2f, true);
                    if (t.childCount > 0)
                    {
                        t.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.2f, true);
                    }
                }
            }
            else
            {
                Time.timeScale = 0;
                paused = true;
                foreach (Transform t in pauseMenu.transform)
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

    public void StartGame()
    {
        string currentScene = GameManager.instance.Load();
        if (currentScene != "")
        {
            GameManager.instance.LoadScene(currentScene);
        }
        else
        {
            GameManager.instance.LoadScene("Neighborhood0");
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
