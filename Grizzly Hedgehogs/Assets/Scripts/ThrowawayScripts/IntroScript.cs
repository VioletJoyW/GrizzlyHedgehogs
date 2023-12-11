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



	public void Init()
	{
		if (gameManager.instance.player == null) return;
		playerController.Intro = true;
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		gameManager.instance.player.transform.localScale = new Vector3(gameManager.instance.player.transform.localScale.x, 2.0f, gameManager.instance.player.transform.localScale.y);
		Camera.main.farClipPlane = 150.0f;

	}

	public void Run()
	{
		if (cinemaCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Wait_And_look_around"))
		{
			cinemaCamera.GetComponent<Animator>().StopPlayback();
			cinemaCamera.SetActive(false);
			gameManager.instance.player.SetActive(true);
			gameManager.instance.playerScript.SpawnPlayer();
			gameManager.instance.ActivatePlayerAtStart = true;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

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
