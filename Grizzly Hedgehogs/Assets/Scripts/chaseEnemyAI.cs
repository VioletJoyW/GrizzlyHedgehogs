using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class chaseEnemyAI : Entity
{
	[Header("----- Audio -----")]
    [Range(0, 1)][SerializeField] float enemyVol;
    [SerializeField] new AudioSource audio;
	[SerializeField] AudioClip[] audioStep;
	[Range(0, 1)][SerializeField] float audioStepVolume;
	[SerializeField] AudioClip[] audioDamage;
	[Range(0, 1)][SerializeField] float audioDamageVolume;
    [SerializeField] AudioClip audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;
    
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Collider damageCollider;
    //[SerializeField] Renderer laser;
    //[SerializeField] Collider[] _ragdollsCollider;
    //[SerializeField] Rigidbody[] _ragdolls;

    [Header("----- Config -----")]
    [SerializeField] bool fromSpawner = false;
    
    [Header("----- Enemy Stats -----")]
	[SerializeField] int hitPoints;
    [SerializeField] int playerFaceSpeed;
    //[SerializeField] int viewCone;
    [SerializeField] int attackCone;
    //[SerializeField] int roamDist;
    //[SerializeField] int roamPauseTime;

    [Header("----- Attack Stats -----")]
    [SerializeField] Collider attackCollider;
    [SerializeField] float attackRate;
    [SerializeField] int attackDamage;

    Vector3 playerDir;
    //bool playerInRange;
    float angleToPlayer;
    float stoppingDistOrig;
    //bool destinationChosen;
    //private Rigidbody rb;
    //Vector3 startingPos;

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

        //disableRag();
		stoppingDistOrig = agent.stoppingDistance;
        //startingPos = transform.position;

        if (!fromSpawner) // If we're not in a spawner, add ourselves to the goal. 
        {
            gameManager.instance.updateEnemyCount(1);
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
                StartCoroutine(PlaySteps(1, agent.velocity.normalized.magnitude, true));
            }

            CanSeePlayer();
        }
    }


    /// <summary>
    /// Cheacks to see if the player can be seen
    /// </summary>
    /// <returns></returns>
    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;

        agent.SetDestination(gameManager.instance.player.transform.position);

        Debug.DrawRay(headPos.position, playerDir);
        //Debug.Log(angleToPlayer);

        if (agent.remainingDistance < agent.stoppingDistance)
        {
            FaceTarget();
            if (angleToPlayer <= attackCone && !isShooting)
            {
                StartCoroutine(Shoot());
            }
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage damagable = other.GetComponent<IDamage>();
        if (damagable != null)
        {
            damagable.TakeDamage(attackDamage);
        }
    }

    protected override IEnumerator Shoot() 
    {
        isShooting = true;

        animator.SetTrigger("Shoot");
        aud.PlayOneShot(audAttack, audAttackVol * enemyVol);

        yield return new WaitForSeconds(attackRate);
        isShooting = false;
    }

    public void SetFromSpawner(bool on)
    {
        fromSpawner = on;
    }

    public void activateAttackCollider(bool on) // Called in animation event
    {
        attackCollider.gameObject.SetActive(on);
    }

    public override void TakeDamage(int amount)
    {
        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol * enemyVol);

        StartCoroutine(DamageReact());
        agent.SetDestination(gameManager.instance.player.transform.position);
    }

    IEnumerator DamageReact()
    {
        float speedOrig = agent.speed;

        model.material.color = Color.red;
        agent.speed = 0;

        yield return new WaitForSeconds(0.2f);

        model.material.color = Color.white;
        agent.speed = speedOrig;
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
