using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Entity
{
	[Header("----- Audio -----")]
    [SerializeField] protected new AudioSource audio;
	[SerializeField] protected AudioClip[] audioStep;
	[Range(0, 1)][SerializeField] protected float audioStepVolume;
	[SerializeField] protected AudioClip[] audioDamage;
	[Range(0, 1)][SerializeField]  protected float audioDamageVolume;
    [SerializeField] protected AudioClip audShoot;
    [Range(0, 1)][SerializeField] protected float audShootVol;
    
    [Header("----- Components -----")]
    [SerializeField] protected Renderer model;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform shootPos;
    [SerializeField] protected Transform headPos;
    [SerializeField] protected Collider damageCollider;
    [SerializeField] protected Renderer laser;
    [SerializeField] protected Renderer mapMarker;
    [SerializeField] protected Collider[] _ragdollsCollider;
    [SerializeField] protected Rigidbody[] _ragdolls;
    [SerializeField] protected Transform _head;

    [Header("----- Config -----")]
    [SerializeField] protected bool fromSpawner = false;
    
    [Header("----- Enemy Stats -----")]
	[SerializeField]protected int hitPoints;
    [SerializeField] protected int playerFaceSpeed;
    [SerializeField] protected int viewCone;
    [SerializeField] protected int shootCone;
    [SerializeField] protected int roamDist;
    [SerializeField] protected int roamPauseTime;

    [Header("----- Gun Stats -----")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float shootRate;




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
        maxHP = hitPoints;
        AudioSource = audio;
        AudioSteps = audioStep;
        AudioStepVolume = audioStepVolume;
        AudioDamage = audioDamage;
        AudioDamageVolume = audioDamageVolume;
        disableRag();
		stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
		IsAlive = true;
        mapMarker.enabled = true;
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
            if (gameManager.instance.bHead)
            {
                bigHead();
            }
            else
            {
                normalHead();
            }
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
    virtual protected bool CanSeePlayer()
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

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
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
        aud.PlayOneShot(audShoot, audShootVol * settingsManager.sm.settingsCurr.enemyVol);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void SetFromSpawner(bool on)
    {
        fromSpawner = on;
    }

    virtual public void CreateBullet() // Called in animation event
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
            if (i < _ragdollsCollider.Length)
            {
                _ragdollsCollider[i].enabled = true;
            }
            else if (_ragdollsCollider[i].CompareTag("Player"))
            {
                _ragdollsCollider[i].enabled = !_ragdollsCollider[i].enabled;
            }
            _ragdolls[i].isKinematic = false;
            {

            }

            if (gameManager.instance.beybladebeybladeLETITRIP)
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
        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol * settingsManager.sm.settingsCurr.enemyVol);

        if (HP <= 0)
        {
            IsAlive = false;
            mapMarker.enabled = false;
            enableRag();
            damageCollider.enabled = false;

            gameManager.instance.updateEnemyCount(-1);

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

    public void bigHead()
    {
        _head.localScale = new Vector3(5, 5, 5);
    }

    public void normalHead()
    {
        _head.localScale = new Vector3(1, 1, 1);
    }

    public void applyForce() // will do it in spare time
    {

    }

    protected virtual IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

   protected virtual void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
}
