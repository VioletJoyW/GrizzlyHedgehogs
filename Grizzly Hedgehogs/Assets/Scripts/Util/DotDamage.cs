using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DotDamage 
{

	static List<_DotDamage> dotDamages = null;
	static int dotObjCount = 0;

	/// <summary>
	/// <para>Will deal DOT damage to an entity.</para><para> Note: This function will auto remove the "DotDamage" object.
	/// If you need to check for when it's done dealing damage, you should use "DealDOTDamageManual".</para>
	/// </summary>
	/// <param name="rate"></param>
	/// <param name="duration"></param>
	/// <param name="maxHealth"></param>
	/// <param name="entity"></param>
	public static void DealDOTDamage(float rate, float duration, int maxHealth, ref Entity entity) 
	{
		if(dotDamages == null) dotDamages = new List<_DotDamage>();
		int id = dotObjCount;

		if (dotDamages.Count < 1 || dotObjCount == dotDamages.Count)
		{
			dotDamages.Add(new _DotDamage(id, rate, duration, maxHealth, ref entity)); // Add a new dot damage for that entity.
		}
		else // We hit this branch if the we have some open slots.
		{
			for (int ndx = 0; ndx < dotDamages.Count; ndx++)
				if (dotDamages[ndx] == null) dotDamages[ndx] = new _DotDamage(id, rate, duration, maxHealth, ref entity);
			
		}
		++dotObjCount;
	}

	/// <summary>
	/// <para>Will deal DOT damage to an entity.</para>
	/// <para>Note: This function will not auto remove the "DotDamage" object, and you must eventually remove it 
	///  with the "RemoveDOTDamage" function.</para>
	/// </summary>
	/// <param name="rate"></param>
	/// <param name="duration"></param>
	/// <param name="maxHealth"></param>
	/// <param name="entity"></param>
	/// <param name="id"></param>
	public static void DealDOTDamageManual(float rate, float duration, int maxHealth, ref Entity entity, out int id) 
	{
		if(dotDamages == null) dotDamages = new List<_DotDamage>();
		id = dotObjCount;

		if (dotDamages.Count < 1 || dotObjCount == dotDamages.Count)
		{
			dotDamages.Add(new _DotDamage(id, rate, duration, maxHealth, ref entity, false)); // Add a new dot damage for that entity.
		}
		else // We hit this branch if the we have some open slots.
		{
			for (int ndx = 0; ndx < dotDamages.Count; ++ndx)
				if (dotDamages[ndx] == null) dotDamages[ndx] = new _DotDamage(id, rate, duration, maxHealth, ref entity, false);
		}
		++dotObjCount;
	}

	/// <summary>
	/// Removes the "DOTDamge" object.
	/// </summary>
	/// <param name="id"></param>
	public static void RemoveDOTDamage(int id) 
	{
		if(dotDamages == null) return;
		dotDamages[id] = null;
		--dotObjCount;
		if(dotObjCount < 1) dotDamages.Clear(); // We only need to clear if our object count is empty.

	}


	static public IEnumerator DealDamage(int id) 
	{
		yield return dotDamages[id].dealDamage();
	}

	/// <summary>
	/// <para>Tells you if dot damge is done being applied.</para>
	/// </summary>
	/// <param name="id"></param>
	/// <returns>
	/// <para>True if done.</para>
	/// <para>Note: This will also return true if 0 Dot objs are made.</para>
	/// </returns>
	public static bool IsDOTDamageDone(int id)
	{
		if (dotDamages == null) return true;
		else return dotDamages[id].IsDoneDealingDamage;
	}

	/// <summary>
	/// A helper class for DOTDamage
	/// </summary>
	private class _DotDamage : MonoBehaviour
	{
		float rate;
		int maxHealth;
		float damage;
		int id;
		bool autoDel;

		Timer timer;
		Entity entity;

		public _DotDamage(int id, float rate, float duration, int maxHealth,ref Entity entity, bool autoDel = true)
		{
			this.id = id;
			this.rate = rate;
			this.maxHealth = maxHealth;
			this.entity = entity;
			this.autoDel = autoDel;
			timer = new Timer(duration);
		}


		public IEnumerator dealDamage()
		{
			while (!timer.IsDone && entity.IsAlive) // While we still have damage to give and the enemy isn't dead.
			{
				if((int) damage > 0) // truncate the float and check the int.
				{
					((IDamage)entity).TakeDamage((int) damage); // Damage the entity if damage larger than 0.
					damage = 0;
				}
				else damage += (maxHealth * rate); // accumulate damage until its atleat >= 1.

				timer.Update(Time.deltaTime);
				yield return null; // pause this funtion call for a sec.
			}
			if(autoDel) RemoveDOTDamage(id); // Remove this object from the damage list.
			yield break;
		}



		/// <summary>
		/// Lets you know when the damage is done being delt. 
		/// </summary>
		public bool IsDoneDealingDamage { get => timer.IsDone; }


	}

}