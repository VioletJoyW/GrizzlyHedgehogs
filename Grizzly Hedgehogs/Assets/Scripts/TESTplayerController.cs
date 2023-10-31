using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTplayerController : MonoBehaviour, iDamage
{
    [SerializeField] CharacterController controller;
    

    [Header("----- Player Stats -----")]
    [Range(1, 20)][SerializeField] int playerHealth;
    [Range(1, 10)][SerializeField] float playerSpeed;
    //Added:
    [Range(1, 3)][SerializeField] float sprintFloat;
    //In case we want to change how much faster sprinting is than walking
    [Range(-10, -30)][SerializeField] float gravityFloat;
    [Range(8, 30)][SerializeField] float jumpHeight;
    //Deleted:
    //[Range(1, 30)][SerializeField] float jumpSpeed;
    //Wasn't being used?
    [Range(1, 4)][SerializeField] int jumpsMax;

    [Header("----- Gun Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] int shootRate;
    //Edit:
    [SerializeField] GameObject bullet;

    //Deleted:
    //private Animator animator;
    //Maybe causing other things to stop working?
    bool isShooting;
    private Vector3 move;
    private int jumpTimes;
    //Deleted:
    //private bool groundedPlayer;
    //Wasn't working?
    private Vector3 playerVelocity;

    void Start()
    {
        //Deleted:
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Deleted:
        //float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? playerSpeed * 2 : playerSpeed;
        //Replaced with sprinting code
        //Edit:
        //Moved "move" code to after check for sprinting code

        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            StartCoroutine(shooting());
        }

        //Added:
        if (Input.GetKeyDown(KeyCode.LeftShift)) //If the sprint key starts being pressed
        {
            playerSpeed *= sprintFloat;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) //If the sprint key stops being pressed
        {
            playerSpeed /= sprintFloat;
        }

        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * playerSpeed);

        //Deleted/Edit:
        //groundedPlayer = isGrounded();
        //Replaced with controller.isGrounded
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
        }

        //Edit:
        if (Input.GetButtonDown("Jump") && jumpTimes < jumpsMax) //Deleted "&& isGrounded()", was overriding code for jumpsMax?
        {
            playerVelocity.y = jumpHeight;
            jumpTimes++;
        }

        playerVelocity.y += gravityFloat * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void takeDamage(int amount)
    { 
        playerHealth -= amount;
        if (playerHealth <= 0)
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

        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
