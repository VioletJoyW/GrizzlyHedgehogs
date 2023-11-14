using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteract
{
	/// <summary>
	/// Checks if the object is interactable.
	/// </summary>
	/// <returns></returns>
	public bool Check();

	/// <summary>
	/// Handles interaction triggering.
	/// </summary>
	public void Interact();
}
