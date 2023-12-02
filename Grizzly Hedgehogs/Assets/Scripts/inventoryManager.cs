using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour 
{
    static List<Iitem> inventory = new List<Iitem>();
    static int inventoryCount;


    static public void AddItem(Iitem item) 
    {
        if (inventory == null)
        {
            return;
        }
        if (inventory.Count < 1 || inventoryCount == inventory.Count)
        {
            item.ID = inventoryCount;
            inventory.Add(item);
        }
        else
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = item;
                    item.ID = i;
                    break;
                }
            }
        }
        inventoryCount++;
    }


	static public void RemoveItem(int _id)
	{
		if(inventoryCount > 0)
        {
            inventory[_id] = null;
            inventoryCount--;
        }
        else
        {
            inventory.Clear();
            inventoryCount = 0;
        }
	}

    public static List<Iitem> Inventory { get => inventory;}
}
