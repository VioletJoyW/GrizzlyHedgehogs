using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]

public class RenderTrigger : MonoBehaviour
{

	[SerializeField] GameObject[] triggerPos;
    [SerializeField][Range(0, 500)] float triggerDistance;
	bool canRender = false;

	Vector3[] distances;

	private void Start()
	{
		distances = new Vector3[triggerPos.Length];
	}


	// Update is called once per frame
	void Update()
    {
		for(int ndx = 0; ndx < distances.Length; ++ndx) 
		{
			Vector3 distance = gameManager.instance.player.transform.position - triggerPos[ndx].transform.position;
			if(canRender || (distance.magnitude <= triggerDistance))
			{
				(transform.GetChild(0).gameObject).SetActive(true);
				break;
			}else (transform.GetChild(0).gameObject).SetActive(canRender);
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		Vector3 distance = gameManager.instance.player.transform.position - transform.position;
		if (other.CompareTag("Player")) 
		{
			//shouldRender  = (distance.magnitude < triggerDistance);
			canRender = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Vector3 distance = gameManager.instance.player.transform.position - transform.position;
		if (other.CompareTag("Player"))
		{
			canRender = false;
			//shouldRender = (distance.magnitude < triggerDistance);
		}
	}



}
