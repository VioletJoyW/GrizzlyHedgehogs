using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntroScript : MonoBehaviour, ISceneScript
{

	[SerializeField] GameObject TV;
	[SerializeField] GameObject oracle;
	[SerializeField] GameObject cinemaCamera;
	[SerializeField] GameObject lifeWorld;
	[SerializeField] GameObject voidWorld;
	[SerializeField] Material voidWorldSky;
	[SerializeField] Material lifeWorldSky;

	bool isDone = false;
	bool isInit = false;
	bool isFinished = false;
	
	Animator animator;

	public IEnumerator WaitForAnimation()
	{
		isFinished = false;
		while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
		{
			yield return null;
		}
		isFinished = true;
	}

	public void Init()
	{
		if (isInit) return;
		if (gameManager.instance.player == null) return;
		if(oracle != null) animator = oracle.GetComponent<Animator>();
		playerController.Intro = true;
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		gameManager.instance.player.transform.localScale = new Vector3(gameManager.instance.player.transform.localScale.x, 2.0f, gameManager.instance.player.transform.localScale.y);
		Camera.main.farClipPlane = 150.0f;
		isInit = true;
	}

	public void Run()
	{
		if (cinemaCamera == null) return;
		if (cinemaCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hold"))
		{
			StartCoroutine(WaitForAnimation());

		}
		else if (animator.GetCurrentAnimatorStateInfo(0).IsName("OracleHold") && isFinished) 
		{
			animator.Play("GoToDoor");
			StartCoroutine(WaitForAnimation());

		}else if(isFinished)
		{
			animator.Play("WaitAtDoor");
			cinemaCamera.GetComponent<Animator>().StopPlayback();
			cinemaCamera.SetActive(false);
			gameManager.instance.player.SetActive(true);
			gameManager.instance.playerScript.SpawnPlayer();
			gameManager.instance.ActivatePlayerAtStart = true;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			animator.StopPlayback();
			isDone = true;
		}




	}

	public void Close()
	{
		if (gameManager.instance.player == null) return;
		Camera.main.farClipPlane = 50.0f;
		Camera.main.clearFlags = CameraClearFlags.SolidColor;
		gameManager.instance.player.transform.localScale = new Vector3(gameManager.instance.player.transform.localScale.x, 1.5f, gameManager.instance.player.transform.localScale.y);
		//Cursor.visible = true;
		//Cursor.lockState = CursorLockMode.Confined;

	}

	public bool IsDone()
	{
		return isDone;
	}


}
