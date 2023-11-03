using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, iDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] LayerMask hide;
    //[SerializeField] Transform torsoPos;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] Color modelColor;
    // Plan on adding damage variable to the npc
    //[SerializeField] int damage;
    [SerializeField] int playerFaceSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    Vector3 playerDir;
    Vector3 coverPos;
    bool isShooting;
    bool playerInRange;
    
    Animator fN;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        fN = GetComponent<Animator>();
        model.material.color = modelColor;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving();
        if (playerInRange)
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                faceTarget();
            }

            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        fN.SetBool("Shoot", true);
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        fN.SetBool("Shoot", false);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = modelColor;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    void isMoving()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            fN.SetBool("Run", true);
        }
        else
        {
            fN.SetBool("Run", false);
        }
    }

    void getToCover()
    {
        // Takes cover if health is half then original
        if (HP == (HP / 2))
        {

        }
    }
}
