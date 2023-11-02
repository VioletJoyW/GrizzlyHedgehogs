using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, iDamage
{
    [SerializeField] CharacterController controller;
    

    [Header("_-_-_- Player Stats -_-_-_")]
    [Range(1, 20)][SerializeField] int playerHealth;
    [Range(1, 10)][SerializeField] float playerSpeed;
    [Range(-10, -30)][SerializeField] float gravityFloat;
    [Range(8, 30)][SerializeField] float jumpHeight;
    [Range(1, 30)][SerializeField] float jumpSpeed;
    [Range(1, 4)][SerializeField] int jumpsMax;

    [Header("_-_-_- Gun Stats -_-_-_")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] int shootRate;
    [SerializeField] GameObject sphere;

    private Animator animator;
    bool isShooting;
    private Vector3 move;
    private int jumpTimes;
    private bool groundedPlayer;
    private Vector3 playerVelocity;
    private int playerHealthOrig;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealthOrig = playerHealth;
        spawnPlayer();
    }

    void Update()
    {
        float moveSpeed = Input.GetButton("Sprint") ? playerSpeed * 2 : playerSpeed;
        //Animation -------------------------------
        bool isMoving = move.magnitude > 0.01f;

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
		//---------------------------------------


        
        // Attack Logic -------------------------------
        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(shooting());
        }
        //-----------------------------------------

		//Movement -------------------------------
		groundedPlayer = isGrounded();
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
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
        //----------------------------------------------------
    }

    public void takeDamage(int amount)
    { 
        playerHealth -= amount;
        updatePlayerUI();

        if(playerHealth <= 0)
        {
            gameManager.instance.youLose();
        }
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

    private bool isGrounded()
    {
        // This Allows the groundedPlayer bool to work correctly, and shows the raycast in the scene viewer
        //Vector3 ray = new Vector3(0, -0.08f, 0);
        //Debug.DrawRay(transform.position, ray, Color.red);
        //return Physics.Raycast(transform.position, Vector3.down, 0.08f);
        return controller.isGrounded; // <-- This seems to work

	}

    public void spawnPlayer()
    {
        controller.enabled = false;
        playerHealth = playerHealthOrig;
        updatePlayerUI();
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHealthBar.fillAmount = (float)playerHealth / playerHealthOrig;
    }

}
