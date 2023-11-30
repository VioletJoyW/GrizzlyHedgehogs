using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Power Stats")]

public class scriptablePowerStats : ScriptableObject
{
	
	[Header("_-_-_-Global Settings_-_-_-")]
	[Tooltip("Controls the maximum powers allowed.")]
	public static byte MAX_POWER_COUNT = 100;
	
	[Header("_-_-_-Settings_-_-_-")]
	[SerializeField] bool isShield;
	
	[Header("_-_-_-Stats_-_-_-")]
	[SerializeField] PowerBuffer.PowerType powerType;
	[SerializeField] int effectMultiplier;
	[SerializeField] int effect;
	[Header("--------Only use this for a very powerful attack/shield--------")]
	[SerializeField] float coolDown = 0;

	protected byte id;

	private void Awake()
	{
		++PowerBuffer.powerCount;
		if (PowerBuffer.powerCount > MAX_POWER_COUNT) throw new Exception("The maximum power count was reached. (MAX COUNT: " + MAX_POWER_COUNT + ")");
		id = (byte) PowerBuffer.powerCount;
	}

	/// <summary>
	/// Gets the id of the power.
	/// </summary>
	public byte ID { get { return id; } }

}
