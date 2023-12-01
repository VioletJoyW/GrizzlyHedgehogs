using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBuffer 
{
	public static short powerCount = -1;
	/// <summary>
	/// Power types. These are flags responsible for dealing damage/shield effects in a specific way.
	/// </summary>
	public enum PowerType 
	{
		None = 0x0,
		NORAMAL = 0x1,
		FLAME = 0x2,
		FROST = 0x4,
		ELECTRICITY = 0x8,
		ELECTRIC_FLAME = ELECTRICITY | FLAME, // <- Use '|' to combine power effects.
		/* Add new power types here ^^^
		 * Try not to go past 64 types, and follow this pattern:
		 * (0x1, 0x2, 0x4, 0x8, 0x10, 0x20, 0x40, 0x80, 0x100, etc...) 
		 * Example:
		 * FLAME = 0x1,
		 * FROST = 0x2,
		 * ELECTRICITY = 0x4,
		 * NEW_TYPE1 = 0x8,
		 * NEW_TYPE2 = 0x10,
		 * etc...
		 */
		DONT_USE_MAX_TYPE_ = ELECTRIC_FLAME + 1 // Change this when adding a new type, example: MAX_TYPE = <New_Type> + 1
		//^^^DO NOT USE THIS AS A TYPE. It may break the system.
	};


	string name;

	protected List<scriptablePowerStats> powerList = null;
	protected scriptablePowerStats currentPower;
	protected bool isActive = false;

	public PowerBuffer() 
	{
		powerList = new List<scriptablePowerStats>();
	}
	public PowerBuffer(string _name) 
	{
		Name = _name;
		powerList = new List<scriptablePowerStats>();
	}

	/// <summary>
	/// Gets the currently active power.
	/// </summary>
	public scriptablePowerStats GetCurrentPower { get => currentPower;}

	/// <summary>
	/// Sets the currently active power.
	/// </summary>
	public int SetCurrentPower { set => currentPower = powerList[value]; }

	/// <summary>
	/// Use this to add a power.
	/// </summary>
	/// <param name="power"></param>
	public void AddPower(scriptablePowerStats power) 
	{
		powerList.Add(power);
		if (currentPower == null) currentPower = powerList[0];
	}

	/// <summary>
	/// Removes a power by its id.
	/// </summary>
	/// <param name="powerID"></param>
	public void RemovePower(ref int powerID)
	{
		powerList.RemoveAt(powerID);
	}

	/// <summary>
	/// Gets the list of currently owned powers.
	/// </summary>
	public List<scriptablePowerStats> PowerList { get => powerList; }// Note: "get => var;"  works the same as "get {return var;}"

	/// <summary>
	/// Gets the owned power count.
	/// </summary>
	public int Count { get { return powerList.Count; } }

	/// <summary>
	/// Tells you if the power buffer is active.
	/// </summary>
	public bool IsActive { get => isActive; set => isActive = value; }

	public string Name { get => "PowerBuffer-" + name; set => name = value; }
}


