using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBehavior : MonoBehaviour
{

    [SerializeField] IntroScript intro;
    [SerializeField] GameObject oracle;
    [SerializeField] Oracle_Intro_start oracleIntroScript;
    [SerializeField] Animator animator;

    bool isFinished = false;
    bool isWaitng = false;
    bool triggerIntroEvent1 = false;
    bool triggerIntroEvent2 = false;
    int timerID = -1;
	public IEnumerator WaitForAnimation()
	{
        isFinished = false;
		while (Mathf.Round(animator.GetCurrentAnimatorStateInfo(0).normalizedTime) < 1.0f)
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
        Utillities.CreateGlobalTimer(6, ref timerID);
	}

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WaitForAnimation());
    }


    

    // Update is called once per frame
    void Update()
    {
        Utillities.UpdateGlobalTimer(timerID);
        if (!triggerIntroEvent1) 
        {
            if (isFinished && !isWaitng)
            {
                animator = oracle.GetComponent<Animator>();
                animator.Play("Run");
                animator = GetComponentInChildren<Animator>();
                animator.Play("Wait_And_look_around");
                isWaitng = true;
                StartCoroutine(WaitForAnimation());
            }
            else if (isWaitng && isFinished) 
            {
                isWaitng = false;
                triggerIntroEvent1 = true;
            };
        }else
        {
            if (!triggerIntroEvent2 && Utillities.IsGlobalTimerDone(timerID)) 
            {
                GetComponent<Animator>().StopPlayback();
                animator.StopPlayback();
                StartCoroutine(RunMiniScript());
            }
        }




    }
}
