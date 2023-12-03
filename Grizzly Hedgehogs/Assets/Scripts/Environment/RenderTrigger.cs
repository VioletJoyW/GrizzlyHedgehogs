using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]

public class RenderTrigger : MonoBehaviour
{


    [SerializeField][Range(0, 500)] float triggerDistance;
	bool shouldRender = false;
	bool canRender = false;

    // Update is called once per frame
    void Update()
    {
        (transform.GetChild(0).gameObject).SetActive(canRender);
	}


	private void OnTriggerEnter(Collider other)
	{
		Vector3 distance = gameManager.instance.player.transform.position - transform.position;
		if (other.CompareTag("Player")) 
		{
			shouldRender  = (distance.magnitude < triggerDistance);
			canRender = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Vector3 distance = gameManager.instance.player.transform.position - transform.position;
		if (other.CompareTag("Player"))
		{
			canRender = false;
			shouldRender = (distance.magnitude < triggerDistance);
		}
	}



}
