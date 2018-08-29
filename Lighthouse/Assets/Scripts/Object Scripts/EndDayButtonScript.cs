using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayButtonScript : MonoBehaviour {

    private Canvas promptCanvas;
    [SerializeField]
    private string nextSceneName;

	// Use this for initialization
	void Start () {
        promptCanvas = this.transform.GetComponent<Canvas>();
	}

    public void EndDay()
    {
        promptCanvas.enabled = false;
        GameManager.instance.LoadScene(nextSceneName);
    }

    public void CloseCanvas()
    {
        promptCanvas.enabled = false;
        GameManager.instance.Player.State = PlayerState.ACTIVE;
    }
	
}
