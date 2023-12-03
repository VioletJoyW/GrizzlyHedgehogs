using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iitem : Iinteract
{
	int ID { get; set; }
	int Inventory_ID { get; set; }
}
