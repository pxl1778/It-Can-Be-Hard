using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {
    [SerializeField]
    private string itemName;
    public string ItemName { get { return itemName; } }

    private bool active = false;
    private GameManager gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active && Input.GetButtonDown("Jump"))
        {
            gm.InventoryMan.AddItemToInventory(itemName);
            Destroy(this.transform.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            active = false;
        }
    }
}
