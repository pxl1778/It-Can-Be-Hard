using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {

	public void BackToTitle()
    {
        GameManager.instance.Globals.Dictionary["mateo1Count"] = 0;
        GameManager.instance.Globals.Dictionary["glowRadius"] = 0.0f;
        GameManager.instance.InventoryMan.resetInventory();
        GameManager.instance.StartLoadScene("Title");
    }
}
