using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Entity
{
	[Header("----- Audio -----")]
    [Range(0, 1)][SerializeField] float enemyVol;
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
    [SerializeField] Renderer laser;
    [SerializeField] Collider[] _ragdollsCollider;
    [SerializeField] Rigidbody[] _ragdolls;

    [Header("----- Config -----")]
    [SerializeField] bool fromSpawner = false;
    
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

    Vector3 playerDir;
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
        AudioStepVolume = audioStepVolume * enemyVol;
        AudioDamage = audioDamage;
        AudioDamageVolume = audioDamageVolume * enemyVol;

        disableRag();
		stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;

        if (!fromSpawner) // If we're not in a spawner, add ourselves to the goal. 
        {
            gameManager.instance.updateGameGoal(1);
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
            
            if (agent.velocity.normalized.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(PlaySteps(1, agent.velocity.normalized.magnitude));
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
    /// Cheacks to see if the player can be seen
    /// </summary>
    /// <returns></returns>
    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(headPos.position, playerDir);
        //Debug.Log(angleToPlayer);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit))
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
        aud.PlayOneShot(audShoot, audShootVol * enemyVol);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void SetFromSpawner(bool on)
    {
        fromSpawner = on;
    }

    public void CreateBullet() // Called in animation event
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void disableRag()
    {
		/* I noticed that "_ragdollsCollider" & "_ragdolls" will usually have the same length. 
        *  So I put them into one loop to save time.
		*/
		for (int i = 0; i < _ragdolls.Length; i++)
		{
			if(i < _ragdollsCollider.Length) _ragdollsCollider[i].enabled = false;
			_ragdolls[i].isKinematic = true;
		}

	}

    public void enableRag()
    {
		/* I noticed that "_ragdollsCollider" & "_ragdolls" will usually have the same length. 
        *  So I put them into one loop to save time.
		*/
		for (int i = 0; i < _ragdolls.Length; i++)
		{
			if (i < _ragdollsCollider.Length) _ragdollsCollider[i].enabled = true;
			_ragdolls[i].isKinematic = false;

            if(gameManager.instance.beybladebeybladeLETITRIP)
            {
                Vector3 physicsForce = transform.position - gameManager.instance.player.transform.position;
                if (physicsForce != null)
                {
                    return;
                }
                rb.AddForce(physicsForce.normalized * physicsForce.magnitude, ForceMode.VelocityChange);
            }
        }
	}

    public override void TakeDamage(int amount)
    {
        HP -= amount;
        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol * enemyVol);

        if (HP <= 0)
        {
            enableRag();
            damageCollider.enabled = false;

            gameManager.instance.updateGameGoal(-1);

            agent.enabled = false;
            animator.enabled = false;
            if(gameObject.CompareTag("Enemy Sniper"))
            {
                laser.enabled = false;
            }
        }
        else
        {
            StartCoroutine(FlashRed());
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    public void ChangeEnemyVol(float volume)
    {
        enemyVol = volume;
        AudioStepVolume = audioStepVolume * enemyVol;
        AudioDamageVolume = audioDamageVolume * enemyVol;
    }
}
