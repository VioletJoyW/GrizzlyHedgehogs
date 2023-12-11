using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger2 : MonoBehaviour
{
    [SerializeField] float triggerTime = 5f;
	[SerializeField] GameObject orb;
	[SerializeField] GameObject orbPos;


	int tiemrID = -1;

	private void Awake()
	{
		Utillities.CreateGlobalTimer(triggerTime, ref tiemrID);
		orb.transform.localPosition = orbPos.transform.localPosition;
	}

	// Update is called once per frame
	void Update()
    {
		Utillities.UpdateGlobalTimer(tiemrID);

		if(Utillities.IsGlobalTimerDone(tiemrID))
		{
			SceneLoaderObj.IsDown = true;
			SceneLoaderObj.Fade(1f, true);
		}
    }
}
