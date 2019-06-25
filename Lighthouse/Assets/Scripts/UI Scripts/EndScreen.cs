using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {

	public void BackToTitle()
    {
        //GameManager.instance.Globals.Dictionary[GlobalData.CHARACTER_COUNTS] = 0;
        GameManager.instance.Globals.Dictionary[GlobalData.GLOW_RADIUS] = 0.0f;
        GameManager.instance.InventoryMan.resetInventory();
        GameManager.instance.StartLoadScene("Title");
    }
}
