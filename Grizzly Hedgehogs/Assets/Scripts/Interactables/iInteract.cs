using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteract
{
	/// <summary>
	/// Checks if the object's interactions are unlocked.
	/// </summary>
	/// <returns></returns>
	public bool CheckUnlocked();

	/// <summary>
	/// Handles interaction triggering.
	/// </summary>
	public void Interact();
}
