using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTutorial : MonoBehaviour {

    [SerializeField]
    private int TutorialIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject.Find("TutorialCanvas").GetComponent<TutorialCanvasScript>().FadeInTutorial(TutorialIndex);
            Debug.Log("trigger hit");
            Destroy(this.gameObject);
        }
    }
}
