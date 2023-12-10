using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour, ISceneScript
{


	public void Init()
	{
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		gameManager.instance.player.transform.localScale = new Vector3(gameManager.instance.player.transform.localScale.x, 2.0f, gameManager.instance.player.transform.localScale.y);
		Camera.main.farClipPlane = 150.0f;
	}

	public void Run()
	{
		
	}

	public void Close()
	{
		Camera.main.farClipPlane = 50.0f;
		Camera.main.clearFlags = CameraClearFlags.SolidColor;
		gameManager.instance.player.transform.localScale = new Vector3(gameManager.instance.player.transform.localScale.x, 1.5f, gameManager.instance.player.transform.localScale.y);
	}
}
