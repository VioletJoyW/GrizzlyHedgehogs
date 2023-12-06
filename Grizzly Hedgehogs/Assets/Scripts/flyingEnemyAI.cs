using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class flyingEnemyAI : EnemyAI
{

    //[Header("----- Audio -----")]
    //[SerializeField] new AudioSource audio;
    //[SerializeField] AudioClip[] audioStep;
    //[Range(0, 1)][SerializeField] float audioStepVolume;
    //[SerializeField] AudioClip[] audioDamage;
    //[Range(0, 1)][SerializeField] float audioDamageVolume;
    //[SerializeField] AudioClip audShoot;
    //[Range(0, 1)][SerializeField] float audShootVol;


    //[Header("----- Components -----")]
    //[SerializeField] Renderer model;
    //[SerializeField] NavMeshAgent agent;
    //[SerializeField] Transform shootPos;
    //[SerializeField] Collider damageCollider;


    //[Header("----- Config -----")]
    //[SerializeField] bool canAddToGoal = false;

    //[Header("----- Enemy Stats -----")]
    //[SerializeField] int hitPoints;
    //[SerializeField] int playerFaceSpeed;
    //[SerializeField] int viewCone;
    //[SerializeField] int shootCone;
    //[SerializeField] int roamDist;
    //[SerializeField] int roamPauseTime;

    //[Header("----- Gun Stats -----")]
    //[SerializeField] GameObject bullet;
    //[SerializeField] float shootRate;


    float flightHeight;
    [SerializeField] float triggerDistance = 1;
    [SerializeField] float descendSpeed = 1;
    [SerializeField] float ascendSpeed = 1;
    [SerializeField] GameObject model_obj;
    [SerializeField] GameObject foot;

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

        model = model_obj.GetComponent<MeshRenderer>();
        flightHeight = model_obj.transform.localPosition.y;
		//headPos.transform.localPosition = new Vector3(model_obj.transform.localPosition.x, flightHeight ,model_obj.transform.localPosition.y);
		stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
        if (!fromSpawner) // If we're not in a spawner, add ourselves to the goal. 
            gameManager.instance.updateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            //animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (agent.velocity.normalized.magnitude > 0.3f && !isPlayingSteps)
            {
               // StartCoroutine(PlaySteps(1, agent.velocity.normalized.magnitude, true));
            }

            if (CanSeePlayer())
            {
                StartCoroutine(Chase());
                Debug.Log("Distance: " + agent.remainingDistance);
                if (agent.remainingDistance < triggerDistance)
                {
                    model_obj.transform.localPosition = Vector3.Lerp(model_obj.transform.localPosition, foot.transform.localPosition, Time.deltaTime * descendSpeed);
                    agent.acceleration -= agent.velocity.normalized.magnitude;
                }
                else 
                {
                    model_obj.transform.localPosition = Vector3.Lerp(model_obj.transform.localPosition, headPos.transform.localPosition, Time.deltaTime * ascendSpeed);
                }
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
            Vector3 pos = hit.position;
            pos.y += flightHeight;
            hit.position = pos;
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    IEnumerator Chase() 
    {
        yield return new WaitForEndOfFrame();
        if (agent.remainingDistance > .5f) 
        {
            agent.acceleration += agent.velocity.normalized.magnitude;
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }


	/// <summary>
	/// Cheacks to see if the player acn be seen
	/// </summary>
	/// <returns></returns>
	protected override bool CanSeePlayer() 
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(transform.position, playerDir);
        //Debug.Log(angleToPlayer);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerDir, out hit, Mathf.Infinity))
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

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    protected override IEnumerator Shoot()
    {
        isShooting = true;;
       // aud.PlayOneShot(audShoot, audShootVol);
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    public void SetCanAddToGoal(bool _b)
    {
		fromSpawner = _b;
    }

    public override void CreateBullet() // Called in animation
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public override void TakeDamage(int amount)
    {
        HP -= amount;
        //aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);

        if (HP <= 0)
        {
            damageCollider.enabled = false;
            gameManager.instance.updateEnemyCount(-1);
            agent.enabled = false;
            Vector3 physicsForce = transform.position - gameManager.instance.player.transform.position;
            if (physicsForce != null)
            {
                return;
            }
            //rb.AddForce(physicsForce.normalized * physicsForce.magnitude, ForceMode.Impulse);
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

    protected override IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    protected override void FaceTarget()
    {
        Vector3 direction = gameManager.instance.player.transform.position - transform.position; // For enemy position
        Quaternion rot = Quaternion.LookRotation(direction);
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
