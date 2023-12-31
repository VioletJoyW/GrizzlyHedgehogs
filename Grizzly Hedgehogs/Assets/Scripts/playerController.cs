using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class playerController : Entity
{
    [Header("_-_-_- Components -_-_-_")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource audioSource;
    
    [Header("_-_-_- Player Stats -_-_-_")]
    [Range(1, 20)][SerializeField] int health;
    [Range(1, 20)][SerializeField] float currentStamina;
    [SerializeField] float crouchSpeed;
    [Range(-10, -30)][SerializeField] float gravityFloat;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [SerializeField] int visionDistance;
    [SerializeField] float restoreStaminaRate;
    [SerializeField] float drainStaminaRate;
    [SerializeField] scriptablePowerStats[] powerStats; // A list of powers set in the inspector. Not meant to be used outside of initialization.

	[Header("_-_-_- Armor & Guns -_-_-_")]
    [SerializeField] ScriptableArmorStats playerArmor;
    [SerializeField] List<ScriptableGunStats> gunsList;
    [SerializeField] GameObject gunModel;

    [Header("_-_-_- Audio -_-_-_")]
    [Range(0, 1)][SerializeField] float playerVol;
    [Range(0, 1)][SerializeField] float objectVol;
    [SerializeField] AudioClip[] audioDamage;
    [Range(0, 1)][SerializeField] float audioDamageVolume;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audioStep;
    [Range(0, 1)][SerializeField] float audioStepVolume;
    [SerializeField] AudioClip audReload;
    [Range(0, 1)][SerializeField] float audReloadVol;
    [SerializeField] AudioClip audHeal;
    [Range(0, 1)][SerializeField] float audHealVol;
    [SerializeField] AudioClip audLock;
    [Range(0, 1)][SerializeField] float audLockVol;

    private List<gunAmmo> ammoTrack = new List<gunAmmo>();
    //private InventoryManager inventoryManager;

    private int selectedGun = 0;
    private int jumpTimes;
    private int pBFButtonCoolDownTimerID; // Power Buffer button cool down timer. (used for delaying key press)

	private bool isRunning;
    private bool isRestoringStamina;
    private bool isCrouching;
    private bool isCrouchingActive;

    private float lastCameraYPos;
    private float damColliderLastHeight;

    private Vector3 move;
    private Vector3 playerVelocity;

    public static bool Intro = false;

    private class gunAmmo
    {
        int currentAmmo;
        int totalAmmo;
        string gunName;
        public gunAmmo(string name, int currammo, int totAmmo)
        {
            gunName = name;
            currentAmmo = currammo;
            totalAmmo = totAmmo;
        }

        public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
        public int TotalAmmo { get => totalAmmo; set => totalAmmo = value; }
        public string Name { get => Name; }
    }

	private void Awake()
	{

	}

	void Start()
    {
		//Setting Entity vars
		HitPoints = health;
		AudioSource = audioSource;
		AudioSteps = audioStep;
		AudioStepVolume = audioStepVolume * playerVol;
		AudioDamage = audioDamage;
		AudioDamageVolume = audioDamageVolume * playerVol;
        powerBuffer = new PowerBuffer(name);
        foreach(var power in powerStats) // Init set powers
        {
            power.Init(); // <- This needs to be called since Unity doesn't want to call awake in a scriptable for some reason.
            powerBuffer.AddPower(power);
        }
        //--------------------------------------------------
        
        //Create a timer for the button press and set the cool down for half a second.
        Utillities.CreateGlobalTimer(.3f, ref pBFButtonCoolDownTimerID);

		lastCameraYPos = Camera.main.transform.localPosition.y;
		damColliderLastHeight = controller.height;
		
        SpawnPlayer();
        ChangeGunModel();
	}

    /// <summary>
    /// Spawns the player at the player spawn postion.
    /// </summary>
    public void SpawnPlayer()
    {
        controller.enabled = false;
        HP = playerArmor.healthMax;
        currentStamina = playerArmor.staminaMax;
        for(int i = 0; i < gunsList.Count; i++)
        {
            gunsList[i].ammoCurrent = gunsList[i].ammoMax;
            ammoTrack.Add(new gunAmmo(gunsList[i].name, gunsList[i].ammoCurrent, gunsList[i].ammoTotal));
        }
        gameManager.instance.UpdatePlayerUI(HP, playerArmor.healthMax, currentStamina, playerArmor.staminaMax, gunsList[selectedGun], ammoTrack[selectedGun].TotalAmmo);
        transform.position = gameManager.instance.playerSpawnPos.transform.localPosition;
        transform.rotation = gameManager.instance.playerSpawnPos.transform.localRotation;
        controller.enabled = true;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        { 

            if(Input.GetKeyUp(settingsManager.sm.settingsCurr.powerBtnToggle))
                powerBuffer.IsActive = !powerBuffer.IsActive;

            Movement();

            Interactions();

            //Update the timer.
            Utillities.UpdateGlobalTimer(pBFButtonCoolDownTimerID);
            SelectPower();
            SelectGun();

            if (Input.GetKeyDown(settingsManager.sm.settingsCurr.reload)) StartCoroutine(ReloadGun());
            else if (Input.GetKeyDown(settingsManager.sm.settingsCurr.shoot) && !isShooting)
				if (!Intro) StartCoroutine(Shoot());
            

            gameManager.instance.UpdatePlayerUI(HP, playerArmor.healthMax, currentStamina, playerArmor.staminaMax, gunsList[selectedGun], ammoTrack[selectedGun].TotalAmmo);
        }
    }

    /// <summary>
    /// Moves the plyaer.
    /// </summary>
    void Movement()
    {

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
        }

        float moveSpeed;

        // Sprint code
        if (Input.GetKey(settingsManager.sm.settingsCurr.sprint) && currentStamina > 0.2f)
        {
            if (!isRunning && !Intro)
                StartCoroutine(Sprint());

            moveSpeed = playerArmor.sprintSpeed;
        }
        else
        {
            moveSpeed = playerArmor.speed;
        }
        // Crouch code
        if (Input.GetKey(settingsManager.sm.settingsCurr.crouch) && !isCrouching && jumpTimes == 0)
        {
            if (!isCrouchingActive)
            {

                isCrouchingActive = true;
            }
            Vector3 pos = Camera.main.transform.localPosition;
            controller.height = Mathf.Lerp(controller.height, damColliderLastHeight * .5f, Time.deltaTime * crouchSpeed);
            pos.y = Mathf.Lerp(Camera.main.transform.localPosition.y, lastCameraYPos * .5f, Time.deltaTime * crouchSpeed);
            
            Camera.main.transform.localPosition = pos;
            isCrouching = Camera.main.transform.localPosition.y < (lastCameraYPos * .5f);// * 1.1f;
        }
        else if ((isCrouching || isCrouchingActive) && !Input.GetKey(settingsManager.sm.settingsCurr.crouch))
        {
            Vector3 pos = Camera.main.transform.localPosition;
            pos.y = Mathf.Lerp(Camera.main.transform.localPosition.y, lastCameraYPos, Time.deltaTime * 8);
            controller.height = Mathf.Lerp(controller.height, damColliderLastHeight, Time.deltaTime * crouchSpeed);
           
            Camera.main.transform.localPosition = pos;
            isCrouching = false;
            if (Camera.main.transform.localPosition.y >= ((int) lastCameraYPos)) 
            {
                isCrouchingActive = false;
                Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, lastCameraYPos, Camera.main.transform.localPosition.z);
                controller.height = damColliderLastHeight;
            }
        }
        //print("Damage Collider Last hieght: " + damColliderLastHeight);
        //print("Camera Last Height: " + lastCameraYPos);
        // Crouch code END

        float moveX = 0;
        float moveZ = 0;
        if (Input.GetKey(settingsManager.sm.settingsCurr.left))
        {
            moveX = -1;
        }
        else if (Input.GetKey(settingsManager.sm.settingsCurr.right))
        {
            moveX = 1;
        }

        if (Input.GetKey(settingsManager.sm.settingsCurr.backwards))
        {
            moveZ = -1;
        }
        else if (Input.GetKey(settingsManager.sm.settingsCurr.forwards))
        {
            moveZ = 1;
        }

        move = transform.right * moveX + transform.forward * moveZ;

        //move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        //Debug.Log(Input.GetAxis("Horizontal"));

        controller.Move(move * Time.deltaTime * moveSpeed);

        if(!isPlayingSteps && controller.isGrounded && move.normalized.magnitude > 0.3f)
        {
            StartCoroutine(PlaySteps(3, moveSpeed, false));
        }

        if (Input.GetKeyDown(settingsManager.sm.settingsCurr.jump) && jumpTimes < jumpsMax && !isCrouchingActive)
        {
            aud.PlayOneShot(audJump[UnityEngine.Random.Range(0, audJump.Length)], audJumpVol * playerVol);
            playerVelocity.y = playerArmor.jumpHeight;
            jumpTimes++;
        }

        playerVelocity.y += gravityFloat * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        if (!isRestoringStamina && !isRunning && currentStamina < playerArmor.staminaMax)
        {
			if (Intro) return;
			StartCoroutine(RestoreStamina());
        }
    }


    /// <summary>
    /// Processes the interation raycast.
    /// Used for triggering the iteration prompt.
    /// </summary>
    void Interactions()
    {
        if (Camera.main == null)
        {
            return;
        }
        Debug.DrawRay(Camera.main.transform.localPosition, Camera.main.transform.forward * visionDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, visionDistance))
        {
            Iinteract interactable = hit.collider.GetComponent<Iinteract>();
            if (interactable != null)
            {
                if (!interactable.CheckUnlocked())
                {
                    gameManager.instance.ShowPrompt(true, "[Locked]");

                    if (Input.GetKeyDown(settingsManager.sm.settingsCurr.interact))
                    {
                        aud.PlayOneShot(audLock, audLockVol * objectVol);
                    }
                }
                else
                {
                    gameManager.instance.ShowPrompt(true, "Press [" + settingsManager.sm.settingsCurr.interact.ToString() + "] to interact");
                }

                if (Input.GetKeyDown(settingsManager.sm.settingsCurr.interact))
                {
                    interactable.Interact();
                }
                return;
            }
        }
        gameManager.instance.ShowPrompt(false);
    }

    /// <summary>
    /// Handles sprinting.
    /// </summary>
    /// <returns></returns>
    IEnumerator Sprint()
    {
        
        isRunning = true;

        currentStamina -= 1;

        yield return new WaitForSeconds(drainStaminaRate);

        isRunning = false;
    }

    /// <summary>
    /// Handles shooting.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Shoot()
    {
        isShooting = true;

        if (ammoTrack[selectedGun].CurrentAmmo > 0)
        {
            RaycastHit hit;
            if (!gameManager.instance.infiniteAmmo) //Part of testing codes
            {
                ammoTrack[selectedGun].CurrentAmmo -= 1;
            }
            if(Camera.main == null)
            {
                yield return null;
            }
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, gunsList[selectedGun].shootDistance))
            {
                Instantiate(gunsList[selectedGun].hitEffect, hit.point, gunsList[selectedGun].hitEffect.transform.rotation);

                IDamage damageable = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform && damageable != null)
                {
					//Here I�m adding the "Power Buffer" damage to the guns� damage.
					bool canUsePB = (powerBuffer.IsActive && !powerBuffer.GetCurrentPower.IsShield);
                
					damageable.TakeDamage(gunsList[selectedGun].shootDamage + ( canUsePB? powerBuffer.GetCurrentPower.Effect * powerBuffer.GetCurrentPower.EffectMultiplier : 0));
                    if (canUsePB) 
                    {
                        //A simple implementation of the Power Dot Damage.
                        switch (powerBuffer.GetCurrentPower.Power) 
                        {
                            case PowerBuffer.PowerType.FLAME:
                                {
                                    int id = -1;
									Entity entity = (Entity)damageable;
									DotDamage.DealDOTDamageManual(powerBuffer.GetCurrentPower.DotRate, powerBuffer.GetCurrentPower.DotDuration, entity.MaxHP, ref entity, out id);
                                    yield return DotDamage.DealDamage(id);
									DotDamage.RemoveDOTDamage(id);
								}
                                break;

							case PowerBuffer.PowerType.FROST:
								{
									int id = -1;
									Entity entity = (Entity)damageable;
									DotDamage.DealDOTDamageManual(powerBuffer.GetCurrentPower.DotRate, powerBuffer.GetCurrentPower.DotDuration, entity.MaxHP, ref entity, out id);
									yield return DotDamage.DealDamage(id);
                                    DotDamage.RemoveDOTDamage(id);
								}
								break;

							case PowerBuffer.PowerType.ELECTRICITY:
								{
									int id = -1;
									Entity entity = (Entity)damageable;
									DotDamage.DealDOTDamageManual(powerBuffer.GetCurrentPower.DotRate, powerBuffer.GetCurrentPower.DotDuration, entity.MaxHP, ref entity, out id);
									yield return DotDamage.DealDamage(id);
									DotDamage.RemoveDOTDamage(id);
								}
								break;

							case PowerBuffer.PowerType.ELECTRIC_FLAME:
								{
									int id = -1;
									Entity entity = (Entity)damageable;
									DotDamage.DealDOTDamageManual(powerBuffer.GetCurrentPower.DotRate, powerBuffer.GetCurrentPower.DotDuration, entity.MaxHP, ref entity, out id);
									yield return DotDamage.DealDamage(id);
									DotDamage.RemoveDOTDamage(id);
								}
								break;

							default:
                                break;
						}
                        currentStamina -= powerBuffer.GetCurrentPower.StaminaCost;
                    }
                }
            }
            aud.PlayOneShot(gunsList[selectedGun].shootSound, gunsList[selectedGun].shootSoundVol * objectVol);
            yield return new WaitForSeconds(gunsList[selectedGun].shootRate);
            isShooting = false;
        }
        else
        {
            StartCoroutine(gameManager.instance.AmmoFlashRed());
            aud.PlayOneShot(gunsList[selectedGun].emptySound, gunsList[selectedGun].emptySoundVol * objectVol);
            yield return new WaitForSeconds(.5f);
            isShooting = false;
        }
        gunsList[selectedGun].ammoCurrent = ammoTrack[selectedGun].CurrentAmmo;
    }

	/// <summary>
	/// Reloads the gun.
	/// </summary>
	IEnumerator ReloadGun() 
    {
        int currentAmmo = ammoTrack[selectedGun].CurrentAmmo;
        int maxAmmo = gunsList[selectedGun].ammoMax;
        int totalAmmo = ammoTrack[selectedGun].TotalAmmo;
		if (!isShooting && currentAmmo < maxAmmo && totalAmmo > 0) 
        {
            int reloadAmount = maxAmmo - currentAmmo;

            if (reloadAmount <= totalAmmo) 
            {
                ammoTrack[selectedGun].TotalAmmo -= reloadAmount;
                ammoTrack[selectedGun].CurrentAmmo += reloadAmount;
            }
            else // We use this branch if the total amount is less than the reload amount. 
            {
                ammoTrack[selectedGun].TotalAmmo = 0;
                ammoTrack[selectedGun].CurrentAmmo += totalAmmo;
			}
            float oldPitch = aud.pitch;
            aud.pitch = .08f;
            aud.PlayOneShot(gunsList[selectedGun].emptySound, gunsList[selectedGun].emptySoundVol * objectVol);
            aud.pitch = oldPitch;
        }
        gunsList[selectedGun].ammoCurrent = ammoTrack[selectedGun].CurrentAmmo;
        yield return new WaitForSeconds(.5f); // Stops spam.
    }


    /// <summary>
    /// Restores stamina over time.
    /// </summary>
    /// <returns></returns>
    IEnumerator RestoreStamina()
    {
        isRestoringStamina = true;
        currentStamina += 1;
        yield return new WaitForSeconds(playerArmor.restoreStaminaRate);
        isRestoringStamina = false;
    }

    public override void TakeDamage(int amount)
    {
        if (!gameManager.instance.playerUnkillable) //Part of testing codes
        {
            // If the "Power Buffer" is active & it's a shield, use the effect value to subtract some damage.
            bool canUsePB = (powerBuffer.IsActive && powerBuffer.GetCurrentPower.IsShield && currentStamina > 0);

			HP -= Mathf.Max(0, amount - (canUsePB ? powerBuffer.GetCurrentPower.Effect * powerBuffer.GetCurrentPower.EffectMultiplier : 0));
            if (canUsePB) 
                currentStamina = Mathf.Max(0, currentStamina - powerBuffer.GetCurrentPower.StaminaCost);


		}

        aud.PlayOneShot(audDamage[UnityEngine.Random.Range(0, audDamage.Length)], audDamageVol * playerVol);

        StartCoroutine(gameManager.instance.PlayerFlashDamage());

        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    /// <summary>
    /// Adds to current health.
    /// </summary>
    /// <param name="amount"></param>
    public void AddHealth(int amount)
    {
        aud.PlayOneShot(audHeal, audHealVol * playerVol);

        HP += amount;

        if (HP > playerArmor.healthMax)
        { 
            HP = playerArmor.healthMax;
        }
    }

    /// <summary>
    /// Gets curent health.
    /// </summary>
    /// <returns></returns>
    public int GetHealth() 
    {
        return HP;
    }

    /// <summary>
    /// Adds to the current ammo.
    /// </summary>
    /// <param name="amount"></param>
    public void AddAmmo(int amount)
    {
		if (Intro) return;
		aud.PlayOneShot(audReload, audReloadVol * objectVol);

        ammoTrack[selectedGun].TotalAmmo += amount;

        if(gunsList[selectedGun].ammoCurrent > gunsList[selectedGun].ammoMax)
        {
            ammoTrack[selectedGun].CurrentAmmo = gunsList[selectedGun].ammoMax;
        }
    }

    /// <summary>
    /// Changes the player's armor.
    /// </summary>
    /// <param name="armor"></param>
    public void ChangeArmor(ScriptableArmorStats armor)
    {
        playerArmor = armor;
    }

    /// <summary>
    /// Adds a gun to the gun inventory (gunList).
    /// </summary>
    /// <param name="gun"></param>
    public void AddGun(ScriptableGunStats gun)
    {
        gunsList.Add(gun);
        selectedGun = gunsList.Count - 1;
        ChangeGunModel();
    }

	/// <summary>
	/// Selects a gun through the scroll wheel.
	/// </summary>
	void SelectGun()
    {
		if (Intro) return;
		//TODO-DejaKill: Make the gun scrolling loop?
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunsList.Count - 1)
        {
            selectedGun++;
            ChangeGunModel();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            ChangeGunModel();
        }
    }

    void SelectPower() 
    {
        if (Intro) return;
        if (powerBuffer.IsActive) 
        {
            //print("Selected Power: "+ powerBuffer.GetCurrentPower.name);
            gameManager.instance.UpdatePowerText();

			if (powerBuffer.Count > 0)
			{
			    int currentPowerID = powerBuffer.GetCurrentPower.ID;
                int dir = Convert.ToByte(Input.GetKey(settingsManager.sm.settingsCurr.powerBtnScrollUp)) - Convert.ToByte(Input.GetKey(settingsManager.sm.settingsCurr.powerBtnScrollDown));
                

                int index = (currentPowerID + dir) % powerBuffer.Count;
				
                //We don't attempt a selection until the timer is up or until the player does something with it.
				if (dir == 0 || !Utillities.IsGlobalTimerDone(pBFButtonCoolDownTimerID)) return;
				print("Power: " + (currentPowerID + dir));
                powerBuffer.SetCurrentPower = (index < 0) ? powerBuffer.Count - 1 : index;
                Utillities.ResetGlobalTimer(pBFButtonCoolDownTimerID);
			}
		}
        else
        {
            gameManager.instance.UpdatePowerText();
        }

	}

	/// <summary>
	/// Change the current gun to a different gun in the list.
	/// </summary>
	void ChangeGunModel()
    {
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunsList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunsList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void ChangePlayerVol(float volume)
    {
        playerVol = volume;

        AudioStepVolume = audioStepVolume * playerVol;
        AudioDamageVolume = audioDamageVolume * playerVol;
    }

    public void ChangeObjectVol(float volume)
    {
        objectVol = volume;
    }

    public void AddItemToInventory(Iitem item) 
    {
		InventoryManager.AddItem(item);
    }

    public void AddItemsToInventory(Iitem[] items) 
    {
        foreach (Iitem item in items)
          InventoryManager.AddItem(item);
    }

    public PowerBuffer GetPowerBuffer()
    {
        return powerBuffer;
    }

    public float GetPlayerGravity()
    {
        return gravityFloat;
    }

    public void SetPlayerGravity(float grav)
    {
        gravityFloat = grav;
    }
}
