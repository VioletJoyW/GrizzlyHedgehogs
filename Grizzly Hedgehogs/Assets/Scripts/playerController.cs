using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] int crouchSpeed;

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


    private int selectedGun = 0;
    private int jumpTimes;

    private bool isRunning;
    private bool isRestoringStamina;
    private bool isCrouching;
    private bool isCrouchingActive;

    private float lastCameraYPos;
    private float damColliderLastHeight;

    private Vector3 move;
    private Vector3 playerVelocity;

    float lastCameraYPos;
    float damColliderLastHeight;
    bool isCrouchingActive;
    bool isCrouching;

    void Start()
    {
		//Setting Entity vars
		HitPoints = health;
		AudioSource = audioSource;
		AudioSteps = audioStep;
		AudioStepVolume = audioStepVolume * playerVol;
		AudioDamage = audioDamage;
		AudioDamageVolume = audioDamageVolume * playerVol;

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
        }
        gameManager.instance.UpdatePlayerUI(HP, playerArmor.healthMax, currentStamina, playerArmor.staminaMax, gunsList[selectedGun].ammoCurrent, gunsList[selectedGun].ammoMax);
        transform.position = gameManager.instance.playerSpawnPos.transform.localPosition;
        controller.enabled = true;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        { 
            Movement();

            Interactions();

            SelectGun();

            if (Input.GetKeyDown(settingsManager.sm.reload)) StartCoroutine(ReloadGun());
            else if (Input.GetKeyDown(settingsManager.sm.shoot) && !isShooting)
              StartCoroutine(Shoot());
            

            gameManager.instance.UpdatePlayerUI(HP, playerArmor.healthMax, currentStamina, playerArmor.staminaMax, gunsList[selectedGun].ammoCurrent, gunsList[selectedGun].ammoMax);
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
        if (Input.GetKeyDown(settingsManager.sm.sprint) && currentStamina > 0.2f)
        {
            if (!isRunning)
                StartCoroutine(Sprint());

            moveSpeed = playerArmor.sprintSpeed;
        }
        else
        {
            moveSpeed = playerArmor.speed;
        }
        // Crouch code
        if (Input.GetKeyDown(settingsManager.sm.crouch) && !isCrouching && jumpTimes == 0)
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
        else if ((isCrouching || isCrouchingActive) && !Input.GetKeyDown(settingsManager.sm.crouch))
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
        if (Input.GetKey(settingsManager.sm.left))
        {
            moveX = -1;
        }
        else if (Input.GetKey(settingsManager.sm.right))
        {
            moveX = 1;
        }

        if (Input.GetKey(settingsManager.sm.backwards))
        {
            moveZ = -1;
        }
        else if (Input.GetKey(settingsManager.sm.forwards))
        {
            moveZ = 1;
        }

        move = transform.right * moveX + transform.forward * moveZ;

        //move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        //Debug.Log(Input.GetAxis("Horizontal"));

        controller.Move(move * Time.deltaTime * moveSpeed);

        if(!isPlayingSteps && controller.isGrounded && move.normalized.magnitude > 0.3f)
        {
            StartCoroutine(PlaySteps(3, moveSpeed));
        }

        if (Input.GetKeyDown(settingsManager.sm.jump) && jumpTimes < jumpsMax && !isCrouchingActive)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol * playerVol);
            playerVelocity.y = playerArmor.jumpHeight;
            jumpTimes++;
        }

        playerVelocity.y += gravityFloat * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        if (!isRestoringStamina && !isRunning && currentStamina < playerArmor.staminaMax)
        {
            StartCoroutine(RestoreStamina());
        }
    }

	/// <summary>
	/// Plays footstep sounds by a speed factor.
	/// Used for player movement.
	/// </summary>
	/// <param name="moveSpeed"></param>
	/// <returns></returns>

	//}

    /// <summary>
    /// Processes the interation raycast.
    /// Used for triggering the iteration prompt.
    /// </summary>
    void Interactions()
    {
        Debug.DrawRay(Camera.main.transform.localPosition, Camera.main.transform.forward * visionDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, visionDistance))
        {
            Iinteract interactable = hit.collider.GetComponent<Iinteract>();
            if (interactable != null)
            {
                if (!interactable.CheckUnlocked())
                {
                    gameManager.instance.ShowLockedPrompt(true); 
                    gameManager.instance.ShowInteractPrompt(false);

                    if (Input.GetKeyDown(settingsManager.sm.interact))
                    {
                        aud.PlayOneShot(audLock, audLockVol * objectVol);
                    }

                    return;
                }

                gameManager.instance.ShowInteractPrompt(true);
                gameManager.instance.ShowLockedPrompt(false);

                if (Input.GetKeyDown(settingsManager.sm.interact))
                {
                    interactable.Interact();
                }
                return;
            }
        }
        gameManager.instance.ShowLockedPrompt(false);
        gameManager.instance.ShowInteractPrompt(false);
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

        if (gunsList[selectedGun].ammoCurrent > 0)
        {
            RaycastHit hit;
            if (!gameManager.instance.infiniteAmmo) //Part of testing codes
            {
                gunsList[selectedGun].ammoCurrent -= 1;
            }
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, gunsList[selectedGun].shootDistance))
            {
                Instantiate(gunsList[selectedGun].hitEffect, hit.point, gunsList[selectedGun].hitEffect.transform.rotation);

                IDamage damageable = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform && damageable != null)
                {
                    damageable.TakeDamage(gunsList[selectedGun].shootDamage);
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
    }

	/// <summary>
	/// Reloads the gun.
	/// </summary>
	IEnumerator ReloadGun() 
    {
        int currentAmmo = gunsList[selectedGun].ammoCurrent;
        int maxAmmo = gunsList[selectedGun].ammoMax;
        int totalAmmo = gunsList[selectedGun].ammoTotal;
		if (!isShooting && currentAmmo < maxAmmo && totalAmmo > 0) 
        {
            int reloadAmount = maxAmmo - currentAmmo;

            if (reloadAmount <= totalAmmo) 
            {
                gunsList[selectedGun].ammoTotal -= reloadAmount;
                gunsList[selectedGun].ammoCurrent += reloadAmount;
            }
            else // We use this branch if the total amount is less than the reload amount. 
            {
				gunsList[selectedGun].ammoTotal = 0;
				gunsList[selectedGun].ammoCurrent += totalAmmo;
			}
            float oldPitch = aud.pitch;
            aud.pitch = .08f;
            aud.PlayOneShot(gunsList[selectedGun].emptySound, gunsList[selectedGun].emptySoundVol * objectVol);
            aud.pitch = oldPitch;
            
        }
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
            HP -= amount;
        }

        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol * playerVol);

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
        aud.PlayOneShot(audReload, audReloadVol * objectVol);

        gunsList[selectedGun].ammoCurrent += amount;

        if(gunsList[selectedGun].ammoCurrent > gunsList[selectedGun].ammoMax)
        {
            gunsList[selectedGun].ammoCurrent = gunsList[selectedGun].ammoMax;
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

}
