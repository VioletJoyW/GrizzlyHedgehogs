using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : Entity
{
    [Header("_-_-_- Components -_-_-_")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource audioSource;

    [Header("_-_-_- Player Stats -_-_-_")]
    [Range(1, 20)][SerializeField] int health;
    [Range(1, 20)][SerializeField] float currentStamina;

    [Range(-10, -30)][SerializeField] float gravityFloat;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [SerializeField] int visionDistance;
    [SerializeField] float restoreStaminaRate;

    [Header("_-_-_- Armor & Guns -_-_-_")]
    [SerializeField] ScriptableArmorStats playerArmor;
    [SerializeField] List<ScriptableGunStats> gunsList;
    [SerializeField] GameObject gunModel;

    [Header("_-_-_- Audio -_-_-_")]
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

    private bool isRunning;
    private bool isRestoringStamina;

    private Vector3 move;
    private int jumpTimes;
    private Vector3 playerVelocity;

    void Start()
    {
		//Setting Entity vars
		HitPoints = health;
		AudioSource = audioSource;
		AudioSteps = audioStep;
		AudioStepVolume = audioStepVolume;
		AudioDamage = audioDamage;
		AudioDamageVolume = audioDamageVolume;

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
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        { 
            Movement();

            Interactions();

            SelectGun();

            if (Input.GetButton("Fire1") && !isShooting)
            {
                StartCoroutine(Shoot());
            }

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

        if (Input.GetButton("Sprint") && currentStamina > 0.2f)
        {
            moveSpeed = playerArmor.sprintSpeed;
            isRunning = true;
            currentStamina -= 0.02f;
        }
        else
        {
            moveSpeed = playerArmor.speed;
            isRunning = false;
        }

        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * Time.deltaTime * moveSpeed);

        if(!isPlayingSteps && controller.isGrounded && move.normalized.magnitude > 0.3f)
        {
            StartCoroutine(PlaySteps(3, moveSpeed));
        }

        if (Input.GetButtonDown("Jump") && jumpTimes < jumpsMax)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
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
	//IEnumerator PlaySteps(float moveSpeed)
 //   {
 //       isPlayingSteps = true;
 //       aud.PlayOneShot(audStep[Random.Range(0, audStep.Length)], audStepVol);
 //       yield return new WaitForSeconds(3 / moveSpeed);
 //       isPlayingSteps = false;

	//}

    /// <summary>
    /// Processes the interation raycast.
    /// Used for triggering the iteration prompt.
    /// </summary>
    void Interactions()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, visionDistance))
        {
            Iinteract interactable = hit.collider.GetComponent<Iinteract>();
            if (interactable != null)
            {
                if (!interactable.Check())
                {
                    gameManager.instance.ShowLockedPrompt(true);

                    if (Input.GetButtonDown("Interact"))
                    {
                        aud.PlayOneShot(audLock, audLockVol);
                    }

                    return;
                }

                gameManager.instance.ShowInteractPrompt(true);

                if (Input.GetButtonDown("Interact"))
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
    /// Handles shooting.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Shoot()
    {
        isShooting = true;

        if (gunsList[selectedGun].ammoCurrent > 0)
        {
            RaycastHit hit;
            gunsList[selectedGun].ammoCurrent -= 1;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, gunsList[selectedGun].shootDistance))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform && damageable != null)
                {
                    damageable.TakeDamage(gunsList[selectedGun].shootDamage);
                }
            }
            aud.PlayOneShot(gunsList[selectedGun].shootSound, gunsList[selectedGun].shootSoundVol);
            yield return new WaitForSeconds(gunsList[selectedGun].shootRate);
            isShooting = false;
        }
        else
        {
            StartCoroutine(gameManager.instance.AmmoFlashRed());
            aud.PlayOneShot(gunsList[selectedGun].emptySound, gunsList[selectedGun].emptySoundVol);
            yield return new WaitForSeconds(.5f);
            isShooting = false;
        }
    }

    /// <summary>
    /// Restores stamina over time.
    /// </summary>
    /// <returns></returns>
    IEnumerator RestoreStamina()
    {
        isRestoringStamina = true;
        yield return new WaitForSeconds(playerArmor.restoreStaminaRate);
        currentStamina += 1;
        isRestoringStamina = false;
    }

    public override void TakeDamage(int amount)
    {
        HP -= amount;

        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);

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
        aud.PlayOneShot(audHeal, audHealVol);

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
        aud.PlayOneShot(audReload, audReloadVol);

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

}
