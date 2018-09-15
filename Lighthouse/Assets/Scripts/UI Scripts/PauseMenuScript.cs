using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour {

    [SerializeField]
    private Canvas homeCanvas;
    [SerializeField]
    private Canvas optionsCanvas;
    [SerializeField]
    private Canvas controlsCanvas;
    [SerializeField]
    private Canvas textingCanvas;
    [SerializeField]
    private Canvas quitCanvas;
    [SerializeField]
    private AudioSource menuBackBlip;
    [SerializeField]
    private AudioSource menuBlip;
    [SerializeField]
    private AudioSource menuClick;

    public void ExitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void InvertYAxis(bool val)
    {
        GameObject.Find("PlayerCam").GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.m_InvertInput = val;
    }

    public void OpenOptions()
    {
        menuClick.PlayOneShot(menuClick.clip);
        homeCanvas.enabled = false;
        optionsCanvas.enabled = true;
    }

    public void OpenQuit()
    {
        menuClick.PlayOneShot(menuClick.clip);
        //homeCanvas.enabled = false;
        quitCanvas.enabled = true;
    }
    public void OpenControls()
    {
        menuClick.PlayOneShot(menuClick.clip);
        homeCanvas.enabled = false;
        controlsCanvas.enabled = true;
    }
    public void OpenTexting()
    {
        menuClick.PlayOneShot(menuClick.clip);
        homeCanvas.enabled = false;
        textingCanvas.enabled = true;
    }

    public void BackHome()
    {
        homeCanvas.enabled = true;
        optionsCanvas.enabled = false;
        quitCanvas.enabled = false;
        textingCanvas.enabled = false;
        controlsCanvas.enabled = false;
        menuBackBlip.Play();
    }

    public void ResetPhone()
    {
        homeCanvas.enabled = true;
        optionsCanvas.enabled = false;
        quitCanvas.enabled = false;
        textingCanvas.enabled = false;
        controlsCanvas.enabled = false;
    }

    public void PlayBackBlip()
    {
        menuBackBlip.PlayOneShot(menuBackBlip.clip);
    }

    public void PlayTick()
    {
        menuBlip.PlayOneShot(menuBlip.clip);
    }
}
