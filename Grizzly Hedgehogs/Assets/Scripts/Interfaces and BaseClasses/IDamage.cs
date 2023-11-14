using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
	/// <summary>
	/// Used for handling Entity damage.
	/// </summary>
	/// <param name="amount"></param>
	public void TakeDamage(int amount);
}
