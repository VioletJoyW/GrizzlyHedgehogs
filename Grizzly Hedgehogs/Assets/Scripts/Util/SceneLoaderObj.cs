using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SceneLoaderObj : MonoBehaviour
{
	[SerializeField] int currentSceneIndex;
    [SerializeField] string[] scenes;

	bool isFading = false;
	float alphaState = 0f;
	bool isDown = false;
	static SceneLoaderObj currentInstance = null;

	private void Awake()
	{
		RawImage img = gameObject.GetComponent<RawImage>();
		
		Color c = new Color(1f, 1f, 1f, 1f);
		img.color = img.color * c;
		
		SceneLoader.SetScenes(scenes);
		currentInstance = this;
	}


	public static void Fade(float fadeAlpha) 
	{
		if (currentInstance == null) return;
		currentInstance.callFade(fadeAlpha);
	}

	private void Start()
	{
		callFade(0f);
	}

	// Update is called once per frame
	void Update()
    {
		
		if (Input.GetKeyUp(KeyCode.V))
		{
			isDown = true;
			StartCoroutine(fade(1f));
		}
		else if(Input.GetKeyUp(KeyCode.B))
		{
			isDown= false;
			StartCoroutine(fade(1f));
		}
    }

	void callFade(float fadeAlpha) 
	{
		StartCoroutine(fade(fadeAlpha, false));
	}

	IEnumerator fade(float alpha, bool change = true) 
	{
		isFading = true;
		Color c = GetComponent<RawImage>().color * new Color(1f, 1f, 1f, alpha);
		while (GetComponent<RawImage>().color != c) 
		{
			GetComponent<RawImage>().color = Color.Lerp(GetComponent<RawImage>().color, c,  Time.fixedDeltaTime);
			yield return new WaitForEndOfFrame();
		}
		
		alphaState = alpha;
		isFading = false;
		if(change)
		{
			if(isDown)SceneLoader.PlayScene(loopValue(++currentSceneIndex, SceneLoader.Size));
			else SceneLoader.PlayScene(loopValue(--currentSceneIndex, SceneLoader.Size));
		}
	}

	int loopValue(int value, int max)
	{
		return ((value) + max) % max;
	}

}
