using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, iDamage
{
    [SerializeField] CharacterController controller;
    

    [Header("_-_-_- Player Stats -_-_-_")]
    [Range(1, 20)][SerializeField] int playerHealth;
    [Range(1, 20)][SerializeField] float playerStamina;
    [Range(1, 20)][SerializeField] int playerAmmo;
    [Range(1, 10)][SerializeField] float playerSpeed;
    [Range(1, 20)][SerializeField] float sprintSpeed;
    [Range(-10, -30)][SerializeField] float gravityFloat;
    [Range(8, 30)][SerializeField] float jumpHeight;
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
    private int playerHealthOrig;
    private float playerStaminaOrig;
    private int playerAmmoOrig;
    void Start()
    {
        playerHealthOrig = playerHealth;
        playerStaminaOrig = playerStamina;
        playerAmmoOrig = playerAmmo;
        spawnPlayer();
    }
    public void spawnPlayer()
    {
        controller.enabled = false;
        playerHealth = playerHealthOrig;
        playerStamina = playerStaminaOrig;
        playerAmmo = playerAmmoOrig;
        gameManager.instance.updatePlayerUI(playerHealth, playerHealthOrig, playerStamina, playerStaminaOrig, playerAmmo, playerAmmoOrig);
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

            gameManager.instance.updatePlayerUI(playerHealth, playerHealthOrig, playerStamina, playerStaminaOrig, playerAmmo, playerAmmoOrig);
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

        if (Input.GetButton("Sprint") && playerStamina > 0.2f)
        {
            moveSpeed = sprintSpeed;
            isRunning = true;
            playerStamina -= 0.02f;
        }
        else
        {
            moveSpeed = playerSpeed;
            isRunning = false;
        }

        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * Time.deltaTime * moveSpeed);

        if (Input.GetButtonDown("Jump") && jumpTimes < jumpsMax)
        {
            playerVelocity.y = jumpHeight;
            jumpTimes++;
        }

        playerVelocity.y += gravityFloat * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        if (!isRestoringStamina && !isRunning && playerStamina < playerStaminaOrig)
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

        if (playerAmmo > 0)
        {
            RaycastHit hit;
            playerAmmo -= 1;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                iDamage damageable = hit.collider.GetComponent<iDamage>();

                if (hit.transform != transform && damageable != null)
                {
                    damageable.takeDamage(shootDamage);
                }
            }
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
        yield return new WaitForSeconds(restoreStaminaRate);
        playerStamina += 1;
        isRestoringStamina = false;
    }

    public void takeDamage(int amount)
    {
        playerHealth -= amount;
        StartCoroutine(gameManager.instance.playerFlashDamage());

        if (playerHealth <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void addHealth(int amount)
    {
        playerHealth += amount;
        if (playerHealth > playerHealthOrig)
        { 
            playerHealth = playerHealthOrig;
        }
    }

    public void addAmmo(int amount)
    {
        playerAmmo += amount;
        if(playerAmmo > playerAmmoOrig)
        {
            playerAmmo = playerAmmoOrig;
        }
    }
}
