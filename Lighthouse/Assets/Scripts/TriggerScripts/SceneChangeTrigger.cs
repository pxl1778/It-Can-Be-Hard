using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour {

    [SerializeField]
    private string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.instance.StartLoadScene(nextScene);
        }
    }
}
