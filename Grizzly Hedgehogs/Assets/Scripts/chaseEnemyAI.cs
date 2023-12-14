using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class chaseEnemyAI : MonoBehaviour
{
    [SerializeField] float headStart;
    [SerializeField] ParticleSystem waitEffect;

	[Header("----- Audio -----")]
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip audSound;
    [Range(0, 1)][SerializeField] float audSoundVol;
    [SerializeField] AudioClip audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;

    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;

    [Header("----- Attack Stats -----")]
    [SerializeField] float attackRate;
    [SerializeField] int attackDamage;
    [SerializeField] ParticleSystem attackEffect;

    bool isPlayingSound;
    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        agent.enabled = false;
        waitEffect.gameObject.SetActive(true);
        StartCoroutine(giveHeadStart());
	}

    IEnumerator giveHeadStart()
    {
        yield return new WaitForSeconds(headStart);
        waitEffect.gameObject.SetActive(false);
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayingSound)
        { StartCoroutine(playSounds()); }

        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }

    IEnumerator playSounds()
    {
        isPlayingSound = true;
        audio.PlayOneShot(audSound, audSoundVol * settingsManager.sm.settingsCurr.enemyVol);
        yield return new WaitForSeconds(1);
        isPlayingSound = false;
    }

    IEnumerator attackPause()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
        attackEffect.gameObject.SetActive(false);
    }

    IEnumerator attackParticles()
    {
        attackEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        attackEffect.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isAttacking)
        {
            if (other.isTrigger || other.CompareTag("Enemy"))
            {
                return;
            }

            IDamage damagable = other.GetComponent<IDamage>();

            Vector3 playerDir = gameManager.instance.player.transform.position - transform.position;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, playerDir, out hit))
            {
                if (damagable != null && hit.collider.CompareTag("Player"))
                {
                    audio.PlayOneShot(audAttack, audAttackVol * settingsManager.sm.settingsCurr.enemyVol);
                    damagable.TakeDamage(attackDamage);
                    StartCoroutine(attackPause());
                    StartCoroutine(attackParticles());
                }
            }
        }
    }
}
