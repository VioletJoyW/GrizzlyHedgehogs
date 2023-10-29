using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [Header("_-_-_- Player Stats -_-_-_")]
    [Range(1, 20)][SerializeField] int playerHealth;
    [Range(1, 10)][SerializeField] float playerSpeed;
    [Range(-10, -30)][SerializeField] float gravityFloat;
    [Range(8, 30)][SerializeField] float jumpHeight;
    [Range(1, 4)][SerializeField] int jumpsMax;

    [Header("_-_-_- Gun Stats -_-_-_")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] int shootRate;
    [SerializeField] GameObject sphere;


    bool isShooting;
    private Vector3 move;
    private int jumpTimes;
    private bool groundedPlayer;
    private Vector3 playerVelocity;
    void Start()
    {
        
    }

    void Update()
    {
        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            StartCoroutine(shooting());
        }

        groundedPlayer = controller.isGrounded;
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

        playerVelocity.y += gravityFloat * Time.deltaTime;
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
}
