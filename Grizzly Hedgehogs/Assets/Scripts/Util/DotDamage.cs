using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DotDamage 
{

	static List<_DotDamage> dotDamages = null;

	/// <summary>
	/// <para>Will deal DOT damage to an entity.</para><para> Note: This function will auto remove the "DotDamage" object.
	/// If you need to check for when it's done dealing damage, you should use "DealDOTDamageManual".</para>
	/// </summary>
	/// <param name="rate"></param>
	/// <param name="duration"></param>
	/// <param name="maxHealth"></param>
	/// <param name="entity"></param>
	/// <param name="id"></param>
	public static void DealDOTDamage(float rate, float duration, int maxHealth, ref Entity entity) 
	{
		if(dotDamages == null) dotDamages = new List<_DotDamage>();
		int id = dotDamages.Count;
		dotDamages.Add(new _DotDamage(id, rate, duration, maxHealth, ref entity)); // Add a new damage for that entity.
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
		id = dotDamages.Count;
		dotDamages.Add(new _DotDamage(id, rate, duration, maxHealth, ref entity, false)); // Add a new damage for that entity.
	}

	/// <summary>
	/// Removes the "DOTDamge" object.
	/// </summary>
	/// <param name="id"></param>
	public static void RemoveDOTDamage(int id) 
	{
		if( dotDamages == null) return;
		dotDamages.RemoveAt(id);
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
			this.rate = rate;
			this.maxHealth = maxHealth;
			this.entity = entity;
			this.autoDel = autoDel;
			timer = new Timer(duration);
			StartCoroutine(dealDamage());
		}


		IEnumerator dealDamage()
		{
			while (!timer.IsDone)
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
			if(autoDel) dotDamages.RemoveAt(id); // Remove this object from the damage list.
		}

		/// <summary>
		/// Lets you know when the damage is done being delt. 
		/// </summary>
		public bool IsDoneDealingDamage { get => timer.IsDone; }


	}

}