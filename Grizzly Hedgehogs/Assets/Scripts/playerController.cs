using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, iDamage
{
    [SerializeField] CharacterController controller;
    

    [Header("_-_-_- Player Stats -_-_-_")]
    [Range(1, 20)][SerializeField] int playerHealth;
    [Range(1, 20)][SerializeField] float playerStamina;
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

    private Animator animator;
    private enum AnimationState { idle, walking, running }

    private bool isRunning;
    private bool isShooting;
    private bool isRestoringStamina;

    private Vector3 move;
    private int jumpTimes;
    private Vector3 playerVelocity;
    private int playerHealthOrig;
    private float playerStaminaOrig;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealthOrig = playerHealth;
        playerStaminaOrig = playerStamina;
        spawnPlayer();
    }
    public void spawnPlayer()
    {
        controller.enabled = false;
        playerHealth = playerHealthOrig;
        playerStamina = playerStaminaOrig;
        updatePlayerUI();
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    void Update()
    {
        movement();

        interactions();

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(shooting());
        }

        updateAnimation();

    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
        }

        float moveSpeed;

        if (Input.GetButton("Sprint") && playerStamina > 0)
        {
            moveSpeed = sprintSpeed;
            isRunning = true;
            playerStamina -= 0.01f;
            updatePlayerUI();
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
                gameManager.instance.interactPrompt.SetActive(true);

                if (Input.GetButtonDown("Interact"))
                {
                    interactable.interact();
                }
                return;
            }
        }
        gameManager.instance.interactPrompt.SetActive(false);
    }

    IEnumerator shooting()
    {
        RaycastHit hit;
        isShooting = true;
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

    IEnumerator restoreStamina()
    {
        isRestoringStamina = true;
        playerStamina += 1;
        updatePlayerUI();
        yield return new WaitForSeconds(restoreStaminaRate);
        isRestoringStamina = false;
    }

    public void takeDamage(int amount)
    {
        playerHealth -= amount;
        updatePlayerUI();

        if (playerHealth <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void changeHealth(int amount)
    {
        playerHealth += amount;
        if (playerHealth > playerHealthOrig)
        { 
            playerHealth = playerHealthOrig;
        }
        updatePlayerUI();
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHealthBar.fillAmount = (float)playerHealth / playerHealthOrig;
        gameManager.instance.playerStaminaBar.fillAmount = playerStamina / playerStaminaOrig;
    }

    void updateAnimation()
    {
        if(isRunning)
        {
            animator.SetInteger("state", (int)AnimationState.running);
        }
        else if (move.magnitude > .01f)
        {
            animator.SetInteger("state", (int)AnimationState.walking);
        }
        else
        {
            animator.SetInteger("state", (int)AnimationState.idle);
        }
    }

}
