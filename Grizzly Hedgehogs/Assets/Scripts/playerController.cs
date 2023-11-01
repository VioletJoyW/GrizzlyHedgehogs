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
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? playerSpeed * 2 : playerSpeed;
        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * moveSpeed);

        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            StartCoroutine(shooting());
        }


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

        groundedPlayer = isGrounded();
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
        }

        if (Input.GetButtonDown("Jump") && jumpTimes < jumpsMax)
        {
            playerVelocity.y = jumpHeight;
            jumpTimes++;
        }

        if (!isGrounded())
        {
            playerVelocity.y += gravityFloat * Time.deltaTime;
        }
        
        controller.Move(playerVelocity * Time.deltaTime);
        
    }

    public void takeDamage(int amount)
    { 
        playerHealth -= amount;
    }

    IEnumerator shooting()
    {
        RaycastHit hit;
        isShooting = true;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    private bool isGrounded()
    {
        // This Allows the groundedPlayer bool to work correctly, and shows the raycast in the scene viewer
        Vector3 ray = new Vector3(0, -0.08f, 0);
        Debug.DrawRay(transform.position, ray, Color.red);
        return Physics.Raycast(transform.position, Vector3.down, 0.08f);
    }
}
