using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, iDamage
{
    [SerializeField] CharacterController controller;

    [SerializeField] scriptableArmorStats playerArmor;

    [Header("_-_-_- Player Stats -_-_-_")]
    [Range(1, 20)][SerializeField] int currentHealth;
    [Range(1, 20)][SerializeField] float currentStamina;
    [Range(1, 20)][SerializeField] int currentAmmo;

    [Range(-10, -30)][SerializeField] float gravityFloat;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [SerializeField] int visionDistance;
    [SerializeField] float restoreStaminaRate;

    [Header("_-_-_- Gun Stats -_-_-_")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] int shootRate;

    private bool isRunning;
    private bool isShooting;
    private bool isRestoringStamina;

    private Vector3 move;
    private int jumpTimes;
    private Vector3 playerVelocity;

    private int playerAmmoOrig;

    void Start()
    {
        playerAmmoOrig = currentAmmo;
        spawnPlayer();
    }
    public void spawnPlayer()
    {
        controller.enabled = false;
        currentHealth = playerArmor.healthMax;
        currentStamina = playerArmor.staminaMax;
        currentAmmo = playerAmmoOrig;
        gameManager.instance.updatePlayerUI(currentHealth, playerArmor.healthMax, currentStamina, playerArmor.staminaMax, currentAmmo, playerAmmoOrig);
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        { 
            movement();

            interactions();

            if (Input.GetButton("Fire1") && !isShooting)
            {
                StartCoroutine(shooting());
            }

            gameManager.instance.updatePlayerUI(currentHealth, playerArmor.healthMax, currentStamina, playerArmor.staminaMax, currentAmmo, playerAmmoOrig);
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

        if (Input.GetButtonDown("Jump") && jumpTimes < jumpsMax)
        {
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

        if (currentAmmo > 0)
        {
            RaycastHit hit;
            currentAmmo -= 1;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                iDamage damageable = hit.collider.GetComponent<iDamage>();

                if (hit.transform != transform && damageable != null)
                {
                    damageable.takeDamage(shootDamage);
                }
            }
            //gameManager.instance.gunPlayerShot();
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
        else
        {
            StartCoroutine(gameManager.instance.ammoFlashRed());
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
        StartCoroutine(gameManager.instance.playerFlashDamage());

        if (currentHealth <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void addHealth(int amount)
    {
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
        currentAmmo += amount;
        if(currentAmmo > playerAmmoOrig)
        {
            currentAmmo = playerAmmoOrig;
        }
    }

    public void changeArmor(scriptableArmorStats armor)
    {
        playerArmor = armor;
    }

}
