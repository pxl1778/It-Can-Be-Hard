using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    private Dictionary<string, int> inventory;

	// Use this for initialization
	void Start () {
        inventory = new Dictionary<string, int>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetInventory()
    {
        inventory = new Dictionary<string, int>();
    }

    public void AddItemToInventory(string pItemName, int pAmount = 1)
    {
        if (inventory.ContainsKey(pItemName))
        {
            inventory[pItemName] = inventory[pItemName] + pAmount;
        }
        else
        {
            Debug.Log("adding item: " + pItemName);
            inventory.Add(pItemName, pAmount);
        }
    }

    public bool HasItem(string pItemName)
    {
        return inventory.ContainsKey(pItemName);
    }

    public int ItemAmount(string pItemName)
    {
        if (HasItem(pItemName))
        {
            return inventory[pItemName];
        }
        else
        {
            return 0;
        }
    }

    public bool RemoveItemAmount(string pItemName, int pAmount)
    {
        if (!HasItem(pItemName))
        {
            Debug.Log("[InventoryManager] Item does not exist in inventory.");
            return false;
        }
        else if(inventory[pItemName] - pAmount < 0)
        {
            return false;
        }
        else
        {
            inventory[pItemName] = inventory[pItemName] - pAmount;
            return true;
        }
    }

    public string[] GetAllItems()
    {
        return inventory.Keys.ToArray();
    }
}
