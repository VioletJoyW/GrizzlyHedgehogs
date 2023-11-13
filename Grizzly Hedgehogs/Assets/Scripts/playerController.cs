using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, iDamage
{
    [Header("_-_-_- Components -_-_-_")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("_-_-_- Player Stats -_-_-_")]
    [Range(1, 20)][SerializeField] int currentHealth;
    [Range(1, 20)][SerializeField] float currentStamina;

    [Range(-10, -30)][SerializeField] float gravityFloat;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [SerializeField] int visionDistance;
    [SerializeField] float restoreStaminaRate;

    [Header("_-_-_- Armor & Guns -_-_-_")]
    [SerializeField] scriptableArmorStats playerArmor;
    [SerializeField] List<scriptableGunStats> gunsList;
    [SerializeField] GameObject gunModel;

    [Header("_-_-_- Audio -_-_-_")]
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audStep;
    [Range(0, 1)][SerializeField] float audStepVol;
    [SerializeField] AudioClip audReload;
    [Range(0, 1)][SerializeField] float audReloadVol;
    [SerializeField] AudioClip audHeal;
    [Range(0, 1)][SerializeField] float audHealVol;
    [SerializeField] AudioClip audLock;
    [Range(0, 1)][SerializeField] float audLockVol;


    private int selectedGun = 0;

    private bool isRunning;
    private bool isShooting;
    private bool isRestoringStamina;
    private bool isPlayingSteps;

    private Vector3 move;
    private int jumpTimes;
    private Vector3 playerVelocity;

    void Start()
    {
        spawnPlayer();
        changeGunModel();
    }
    public void spawnPlayer()
    {
        controller.enabled = false;
        currentHealth = playerArmor.healthMax;
        currentStamina = playerArmor.staminaMax;
        for(int i = 0; i < gunsList.Count; i++)
        {
            gunsList[i].ammoCurrent = gunsList[i].ammoMax;
        }
        gameManager.instance.updatePlayerUI(currentHealth, playerArmor.healthMax, currentStamina, playerArmor.staminaMax, gunsList[selectedGun].ammoCurrent, gunsList[selectedGun].ammoMax);
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        { 
            movement();

            interactions();

            selectGun();

            if (Input.GetButton("Fire1") && !isShooting)
            {
                StartCoroutine(shooting());
            }

            gameManager.instance.updatePlayerUI(currentHealth, playerArmor.healthMax, currentStamina, playerArmor.staminaMax, gunsList[selectedGun].ammoCurrent, gunsList[selectedGun].ammoMax);
        }
    }

    void movement()
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
            StartCoroutine(playSteps(moveSpeed));
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
            StartCoroutine(restoreStamina());
        }
    }

    IEnumerator playSteps(float moveSpeed)
    {
        isPlayingSteps = true;
        aud.PlayOneShot(audStep[Random.Range(0, audStep.Length)], audStepVol);
        yield return new WaitForSeconds(3 / moveSpeed);
        isPlayingSteps = false;
    }

    void interactions()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, visionDistance))
        {
            iInteract interactable = hit.collider.GetComponent<iInteract>();
            if (interactable != null)
            {
                if (!interactable.checkLock())
                {
                    gameManager.instance.showLockedPrompt(true);

                    if (Input.GetButtonDown("Interact"))
                    {
                        aud.PlayOneShot(audLock, audLockVol);
                    }

                    return;
                }

                gameManager.instance.showInteractPrompt(true);

                if (Input.GetButtonDown("Interact"))
                {
                    interactable.interact();
                }
                return;
            }
        }
        gameManager.instance.showLockedPrompt(false);
        gameManager.instance.showInteractPrompt(false);
    }

    IEnumerator shooting()
    {
        isShooting = true;

        if (gunsList[selectedGun].ammoCurrent > 0)
        {
            RaycastHit hit;
            gunsList[selectedGun].ammoCurrent -= 1;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, gunsList[selectedGun].shootDistance))
            {
                iDamage damageable = hit.collider.GetComponent<iDamage>();

                if (hit.transform != transform && damageable != null)
                {
                    damageable.takeDamage(gunsList[selectedGun].shootDamage);
                }
            }
            aud.PlayOneShot(gunsList[selectedGun].shootSound, gunsList[selectedGun].shootSoundVol);
            yield return new WaitForSeconds(gunsList[selectedGun].shootRate);
            isShooting = false;
        }
        else
        {
            StartCoroutine(gameManager.instance.ammoFlashRed());
            aud.PlayOneShot(gunsList[selectedGun].emptySound, gunsList[selectedGun].emptySoundVol);
            yield return new WaitForSeconds(.5f);
            isShooting = false;
        }
    }

    IEnumerator restoreStamina()
    {
        isRestoringStamina = true;
        yield return new WaitForSeconds(playerArmor.restoreStaminaRate);
        currentStamina += 1;
        isRestoringStamina = false;
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;

        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);

        StartCoroutine(gameManager.instance.playerFlashDamage());

        if (currentHealth <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void addHealth(int amount)
    {
        aud.PlayOneShot(audHeal, audHealVol);

        currentHealth += amount;

        if (currentHealth > playerArmor.healthMax)
        { 
            currentHealth = playerArmor.healthMax;
        }
    }

    public int getHealth() 
    {
        return currentHealth;
    }

    public void addAmmo(int amount)
    {
        aud.PlayOneShot(audReload, audReloadVol);

        gunsList[selectedGun].ammoCurrent += amount;

        if(gunsList[selectedGun].ammoCurrent > gunsList[selectedGun].ammoMax)
        {
            gunsList[selectedGun].ammoCurrent = gunsList[selectedGun].ammoMax;
        }
    }

    public void changeArmor(scriptableArmorStats armor)
    {
        playerArmor = armor;
    }

    public void addGun(scriptableGunStats gun)
    {
        gunsList.Add(gun);
        selectedGun = gunsList.Count - 1;
        changeGunModel();
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunsList.Count - 1)
        {
            selectedGun++;
            changeGunModel();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGunModel();
        }
    }

    void changeGunModel()
    {
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunsList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunsList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

}
