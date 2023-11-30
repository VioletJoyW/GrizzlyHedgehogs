using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{

    static List<Iitem> inventory = new List<Iitem>();


    public void AddItem(Iitem item) 
    {
        item.ID = inventory.Count;
        inventory.Add(item);
    }


	public void RemoveItem(int _id)
	{
		inventory.RemoveAt(_id);
	}

}
