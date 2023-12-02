using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamage
{

	public enum EntityType { NONE, ENEMIES, PLAYER }

	protected int HP;

	protected AudioSource aud;
	protected AudioClip[] audStep;
	protected float audStepVol;
	protected AudioClip[] audDamage;
	protected float audDamageVol;


	protected bool isShooting;
	protected bool isPlayingSteps;

	/// <summary>
	/// This object is null by default. create a new one to use.
	/// </summary>
	protected PowerBuffer powerBuffer = null;

	//Getters and setters
	public int HitPoints { get => HP; set => HP = value; }
	public AudioSource AudioSource { get => aud; set => aud = value; }
	public AudioClip[] AudioSteps { get => audStep; set => audStep = value; }
	public float AudioStepVolume { get => audStepVol; set => audStepVol = value; }
	public AudioClip[] AudioDamage { get => audDamage; set => audDamage = value; }
	public float AudioDamageVolume { get => audStepVol; set => audStepVol = value; }
	public bool IsShooting { get => isShooting; set => isShooting = value; }
	public bool IsPlayingSteps { get => isPlayingSteps; set => isPlayingSteps = value; }


	/// <summary>
	/// Subtracts from HP by an amount.
	/// </summary>
	/// <param name="amount"></param>
	public abstract void TakeDamage(int amount);

	/// <summary>
	/// Allows the enitity to shoot a gun.
	/// </summary>
	/// <returns></returns>
	protected abstract IEnumerator Shoot();

	/// <summary>
	/// Plays walking sound effects.
	/// </summary>
	/// <returns></returns>
	protected IEnumerator PlaySteps(int _time_seconds, float _moveSpeed, bool isEnemy)
    {
		isPlayingSteps = true;
		float volMod = 1;
		if(isEnemy)
		{
			volMod = settingsManager.sm.settingsCurr.enemyVol;
		}
		if(audStep.Length > 0)aud.PlayOneShot(audStep[Random.Range(0, audStep.Length)], audStepVol * volMod);
		yield return new WaitForSeconds(_time_seconds / _moveSpeed);
		isPlayingSteps = false;
	}

	public PowerBuffer PowerBuffer { get => powerBuffer; }

}
