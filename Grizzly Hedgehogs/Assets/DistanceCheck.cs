using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
	[SerializeField] GameObject obj;
	//[SerializeField][Range(0,64)] float checkDistance = 45f;
	int _checked = 0;
    // Update is called once per frame
    void Update()
    {
		if (!Camera.main) return;
		switch (_checked) 
		{
			case 0: StartCoroutine(Check()); break;
		}

	}
	IEnumerator Check()
	{
		Vector3 value = gameManager.instance.player.transform.position - transform.position;
		Vector2 playerDir = new Vector2(value.x, value.z);
		Debug.Log("Dis: " + playerDir.magnitude);
		EnemyAI enemy = obj.GetComponent<EnemyAI>();
		//enemy.StopAllCoroutines();
		enemy.IsShooting = false;
		if (playerDir.magnitude > Camera.main.farClipPlane) obj.SetActive(false);
		else obj.SetActive(true);
		_checked = 1;
		yield return new WaitForSeconds(.5f);
		_checked = 0;

	}

}
