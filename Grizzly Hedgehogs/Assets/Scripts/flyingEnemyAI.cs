using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class flyingEnemyAI : Entity
{

    [Header("----- Audio -----")]
    [SerializeField] new AudioSource audio;
    [SerializeField] AudioClip[] audioStep;
    [Range(0, 1)][SerializeField] float audioStepVolume;
    [SerializeField] AudioClip[] audioDamage;
    [Range(0, 1)][SerializeField] float audioDamageVolume;
    [SerializeField] AudioClip audShoot;
    [Range(0, 1)][SerializeField] float audShootVol;


    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Collider damageCollider;
    [SerializeField] AvatarMask avatarMask;


    [Header("----- Config -----")]
    [SerializeField] bool canAddToGoal = false;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int hitPoints;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewCone;
    [SerializeField] int shootCone;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    AudioSource source;
    Vector3 playerDir;
    Vector3 coverPos;
    bool playerInRange;
    float angleToPlayer;
    float stoppingDistOrig;
    bool destinationChosen;
    private Rigidbody rb;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        //Setting Entity vars
        HitPoints = hitPoints;
        AudioSource = audio;
        AudioSteps = audioStep;
        AudioStepVolume = audioStepVolume;
        AudioDamage = audioDamage;
        AudioDamageVolume = audioDamageVolume;


        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
        if (!canAddToGoal) // If we're not in a spawner, add ourselves to the goal. 
            gameManager.instance.updateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (agent.velocity.normalized.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(PlaySteps(1, agent.velocity.normalized.magnitude, true));
            }

            if (playerInRange && !CanSeePlayer())
            {
                StartCoroutine(Roam());
            }
            else if (!playerInRange)
            {
                StartCoroutine(Roam());
            }
        }
    }

    //IEnumerator PlaySteps() // No longer needed here. (It's in the Entity class now)
    //{
    //    isPlayingSteps = true;
    //    aud.PlayOneShot(audStep[Random.Range(0, audStep.Length)], audStepVol);
    //    yield return new WaitForSeconds(1 / agent.velocity.normalized.magnitude);
    //    isPlayingSteps = false;
    //}

    /// <summary>
    /// Allows the enemy to roam to random locations relative to spawn point.
    /// </summary>
    /// <returns></returns>
    IEnumerator Roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            yield return new WaitForSeconds(roamPauseTime);
            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    /// <summary>
    /// Cheacks to see if the player acn be seen
    /// </summary>
    /// <returns></returns>
    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(headPos.position, playerDir);
        Debug.Log(angleToPlayer);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.stoppingDistance = stoppingDistOrig;
                if (angleToPlayer <= shootCone && !isShooting)
                {
                    StartCoroutine(Shoot());
                }

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    FaceTarget();
                }

                agent.SetDestination(gameManager.instance.player.transform.position);

                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
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
            agent.stoppingDistance = 0;
        }
    }

    protected override IEnumerator Shoot()
    {
        isShooting = true;
        animator.SetTrigger("Shoot");
        aud.PlayOneShot(audShoot, audShootVol);
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    public void SetCanAddToGoal(bool _b)
    {
        canAddToGoal = _b;
    }

    public void CreateBullet() // Called in animation
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public override void TakeDamage(int amount)
    {
        HP -= amount;
        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);

        if (HP <= 0)
        {
            damageCollider.enabled = false;
            gameManager.instance.updateEnemyCount(-1);
            agent.enabled = false;
            animator.enabled = false;
            Vector3 physicsForce = transform.position - gameManager.instance.player.transform.position;
            if (physicsForce != null)
            {
                return;
            }
            rb.AddForce(physicsForce.normalized * physicsForce.magnitude, ForceMode.Impulse);
        }
        else
        {
            StartCoroutine(FlashRed());
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }

    private void Ragdoll()
    {

    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void FaceTarget()
    {
        Vector3 direction = gameManager.instance.player.transform.position - transform.position; // For enemy position
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    void GetToCover()
    {
        // Takes cover if health is half then original
        if (HP == (HP / 2))
        {

        }
    }
}
