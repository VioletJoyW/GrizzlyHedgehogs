using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBehavior : MonoBehaviour
{

    [SerializeField] IntroScript intro;
    [SerializeField] TV tv;
    [SerializeField] GameObject oracle;
    [SerializeField] Oracle_Intro_start oracleIntroScript;
    [SerializeField] Animator animator;
    [SerializeField][Range(1f, 100f)] float lookSpeed = 2f;
    [SerializeField][Range(1f, 100f)] float waitTime;
    [SerializeField][Range(1f, 100f)] float transitionTime;

    public GameObject OBJ;

    bool isFinished = false;
    bool isWaitng = false;
    bool triggerIntroEvent1 = false;
    bool triggerIntroEvent2 = false;
    int fadeTimerID = -1;
    int waitTimerID = -1;
	public IEnumerator WaitForAnimation()
	{
        isFinished = false;
		while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
		{
			yield return null;
		}
        isFinished = true;
	}


    IEnumerator RunMiniScript() 
    {
        while (!intro.IsDone()) 
        {
            intro.Run();
            yield return new WaitForEndOfFrame();
        }

    }

	private void Awake()
	{
        Utillities.CreateGlobalTimer(transitionTime, ref fadeTimerID);
        Utillities.CreateGlobalTimer(waitTime, ref waitTimerID);
	}

	// Start is called before the first frame update
	void Start()
    {
		animator = GetComponent<Animator>();
		StartCoroutine(WaitForAnimation());
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}



    bool triggerOracle = false;
    bool fadeStarted = false;
    bool switchedToPlayer = false;
    void Update()
    {

        if(OBJ != null)
        {
            animator = oracle.GetComponent<Animator>();
            GetComponent<Animator>().enabled = false;
            Quaternion rot = Quaternion.LookRotation((OBJ.transform.position - transform.position));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookSpeed);
            if (!triggerOracle)
            {
                animator.SetTrigger("StartRun");
                triggerOracle = true;
                GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 30f, Time.deltaTime * lookSpeed);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("OracleHold")) 
            {
                animator.SetTrigger("HoldDone");
                lookSpeed *= 2;
				GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 50f, Time.deltaTime * lookSpeed);
			
            }
			else if (animator.GetCurrentAnimatorStateInfo(0).IsName("DoorHold"))
            {
                Utillities.UpdateGlobalTimer(waitTimerID);
                if(!fadeStarted && Utillities.IsGlobalTimerDone(waitTimerID))
                {
                    SceneLoaderObj.Fade(1f, false); // Fade out to end the cutscene.
                    fadeStarted = true;
                }
                else Utillities.UpdateGlobalTimer(fadeTimerID);
				GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 25f, Time.deltaTime * (lookSpeed * .5f));
			}

			if (Utillities.IsGlobalTimerDone(fadeTimerID))
            {
                if(!switchedToPlayer)
                {
					gameObject.SetActive(false);
					gameManager.instance.player.SetActive(true);
					gameManager.instance.playerScript.SpawnPlayer();
					gameManager.instance.ActivatePlayerAtStart = true;
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
					SceneLoaderObj.Fade(0f, false); // Fide in to the player.
                    switchedToPlayer = true;
				}
            }
            
        
        }

    }
}
