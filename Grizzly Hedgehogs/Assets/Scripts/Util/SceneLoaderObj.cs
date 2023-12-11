using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;



public interface ISceneScript 
{
	public bool IsDone();
	void Init();
	void Run();
	void Close();
}

public class SceneScriptExecuter 
{
	static List<ISceneScript> sceneScripts = null;
	static SceneScriptExecuter instance = null;

	public static List<ISceneScript> SceneScripts { get => sceneScripts;}
	public static SceneScriptExecuter Instance { get => instance; }

	public static void LoadScripts(ISceneScript[] scripts) 
	{
		if(instance == null)
			instance = new SceneScriptExecuter();
		foreach(ISceneScript scr in scripts)
			sceneScripts.Add(scr);
	}


	public static void RunScripts() 
	{
		if (instance == null || sceneScripts.Count < 1) return;
		bool hasInit = false;
		for (int ndx = 0; ndx < sceneScripts.Count; ++ndx) 
		{
			if (!hasInit) sceneScripts[ndx].Init();
			else sceneScripts[ndx].Run();
			
			if(ndx + 1 >= sceneScripts.Count && !hasInit)
			{
				hasInit = true;
				ndx = 0;
			}
		}
	}

	public static void RunClosing() 
	{
		if (instance == null || sceneScripts.Count < 1) return;

		foreach(ISceneScript scr in sceneScripts)
			scr.Close();


	}


	private SceneScriptExecuter() 
	{
		if(sceneScripts == null)
			sceneScripts = new List<ISceneScript>();
	}





}


public class SceneLoaderObj : MonoBehaviour
{
	[SerializeField] int currentSceneIndex;
    [SerializeField] string[] scenes;
	[SerializeField] GameObject[] scripts;
	[SerializeField][Range(1.0f, 10.0f)] float fadeSpeed = 1.5f;

	bool isFading = false;
	float alphaState = 0f;
	static bool isDown = false;
	public static SceneLoaderObj currentInstance = null;

    public static bool IsDown { get => isDown; set => isDown = value; }
	public float FadeSpeed { get => fadeSpeed; set => fadeSpeed = value; }
    public int CurrentSceneIndex { get => currentSceneIndex; set => currentSceneIndex = value; }

    private void Awake()
	{
		RawImage img = gameObject.GetComponent<RawImage>();
		
		Color c = new Color(1f, 1f, 1f, 1f);
		img.color = img.color * c;
		
		SceneLoader.SetScenes(scenes);
		currentInstance = this;

		ISceneScript[] ss = new ISceneScript[scripts.Length];
		for (int i = 0; i < ss.Length; ++i) 
		{
			ss[i] = scripts[i].GetComponent<ISceneScript>();
		}
		SceneScriptExecuter.LoadScripts(ss);
	}


	public static void Fade(float fadeAlpha, bool change) 
	{
		if (currentInstance == null) return;
		currentInstance.callFade(fadeAlpha, change);
	}

	public static void RunScripts() 
	{
		SceneScriptExecuter.RunScripts();
	}

	private void Start()
	{
		callFade(0f, false);
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

	void callFade(float fadeAlpha, bool change) 
	{
		StartCoroutine(fade(fadeAlpha, change));
	}

	IEnumerator fade(float alpha, bool change = true) 
	{
		isFading = true;
		Color c = GetComponent<RawImage>().color * new Color(1f, 1f, 1f, 0f);
		c.a = alpha;
		while (GetComponent<RawImage>().color != c) 
		{
			GetComponent<RawImage>().color = Color.Lerp(GetComponent<RawImage>().color, c,  Time.fixedDeltaTime * fadeSpeed);
			yield return new WaitForEndOfFrame();
		}
		
		alphaState = alpha;
		isFading = false;
		if(change)
		{
			SceneScriptExecuter.RunClosing();
			if(isDown)SceneLoader.PlayScene(loopValue(++currentSceneIndex, SceneLoader.Size));
			else SceneLoader.PlayScene(loopValue(--currentSceneIndex, SceneLoader.Size));
		}
	}

	int loopValue(int value, int max)
	{
		return ((value) + max) % max;
	}

}
