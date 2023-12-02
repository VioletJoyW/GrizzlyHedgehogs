using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Power Stats")]

public class scriptablePowerStats : ScriptableObject
{
	
	//_-_-_-Global Script Settings_-_-_-
	//Controls the maximum powers allowed.
	public static byte MAX_POWER_COUNT = 10; //<- Set at 10 since I don't know how many powers will be added.  
	
	[Header("_-_-_-Settings_-_-_-")]
	[SerializeField] bool isShield;
	
	[Header("_-_-_-Stats_-_-_-")]
	[Tooltip("Sets the power type.")]
	[SerializeField] PowerBuffer.PowerType power = PowerBuffer.PowerType.None;
	
	[Tooltip("Sets the amount of damage (defense if it's a shield) the entity will have.")]
	[SerializeField] int effect;
	
	[Tooltip("Sets how much the effect is multiplied by.")]
	[SerializeField] int effectMultiplier = 1;
	
	[Tooltip("Sets how much stamina does this power will cost.")]
	[SerializeField] float staminaCost = 1;
	
	[Tooltip("Sets Dot Damage rate.")]
	[SerializeField] float dotRate = 0f;	

	[Tooltip("Sets Dot Damage time length in seconds.")]
	[SerializeField] float dotDuration = 0f;
	

	[Tooltip("Only use this for a very powerful attack/shield!")]
	[SerializeField] float coolDown = 0;

	[Header("_-_-_-AV Effects_-_-_-")]
	[Tooltip("A visual effect for when the power is used (this is optional).")]
	public ParticleSystem visualEffect;


	[Tooltip("A sound that plays when the power is used (this is optional).")]
	public AudioClip soundEffect;

	[Tooltip("Volume of the effect.")]
	[Range(0, 1)] public float effectVol;



	[Tooltip("A sound that plays when the power is turned on (this is optional).")]
	public AudioClip activationSound;

	[Tooltip("Volume of the activation sound effect.")]
	[Range(0, 1)] public float activationSoundVol;



	[Tooltip("A sound that plays when the power is turned off (this is optional).")]
	public AudioClip deactivationSound;

	[Tooltip("Volume of the deactivation sound effect.")]
	[Range(0, 1)] public float deactivationSoundVol;


	protected byte id;
	private bool isInit = false; 
	private void Awake()
	{
		Init();
	}

	public void Init() 
	{
		if(!IsInit)
		{
			IsInit = true;
			++PowerBuffer.powerCount;
			if (PowerBuffer.powerCount > MAX_POWER_COUNT) throw new Exception("The maximum power count was reached. (MAX COUNT: " + MAX_POWER_COUNT + ")");
			id = (byte)PowerBuffer.powerCount;
		}
	}

	//Getters

	/// <summary>
	/// Gets the id of the power.
	/// </summary>
	public byte ID { get { return id; } }

	public bool IsShield { get => isShield; }
	public PowerBuffer.PowerType Power { get => power; }
	public int Effect { get => effect;}
	public int EffectMultiplier { get => effectMultiplier;}
	public float CoolDown { get => coolDown; }
	public float StaminaCost { get => staminaCost; set => staminaCost = value; }
	public bool IsInit { get => isInit; set => isInit = value; }
	public float DotRate { get => dotRate; }
	public float DotDuration { get => dotDuration; set => dotDuration = value; }
}
