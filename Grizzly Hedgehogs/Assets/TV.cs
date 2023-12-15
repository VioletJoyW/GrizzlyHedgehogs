using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class TV : MonoBehaviour, Iinteract
{

	public static bool Ready = true;
    [SerializeField] GameObject tvScreenOverlay;
    [SerializeField] GameObject tvScreen;
    [SerializeField] VideoClip video;
    [SerializeField] bool on = true;
    [SerializeField] bool isWebGL;
    [SerializeField] string videoLink = "";
    bool lastState;
    bool stateChanged;
    bool canRun = false;

	private void Awake()
	{
        lastState = on;
		tvScreen.GetComponent<VideoPlayer>().playOnAwake = false;
		if (tvScreen == null) return;
		if (videoLink == "" || !isWebGL)
		{
			if (video != null) tvScreen.GetComponent<VideoPlayer>().clip = video;
		}
		else
		{
			tvScreen.GetComponent<VideoPlayer>().source = VideoSource.Url;
			tvScreen.GetComponent<VideoPlayer>().url = videoLink;
		}
		tvScreen.SetActive(on);
		tvScreenOverlay.SetActive(!on);
		if (on) 
		{
			tvScreen.GetComponent<VideoPlayer>().Play();
			Ready = tvScreen.GetComponent<VideoPlayer>().isPrepared;
		} 
		
	}


	// Start is called before the first frame update
	void Start()
    {
	    canRun = (tvScreen != null) && ((videoLink != "") || (video != null));
		if (on) Ready = tvScreen.GetComponent<VideoPlayer>().isPrepared;
	}

    // Update is called once per frame
    void Update()
    {
		if (!canRun) return;
        if (!Ready && on) Ready = tvScreen.GetComponent<VideoPlayer>().isPrepared;
        if (stateChanged)
        {
			tvScreen.SetActive(on);
            tvScreenOverlay.SetActive(!on);
            stateChanged = false;
            lastState = on;
        }
        stateChanged = lastState != on;
	}

    public void Toggle()
    {
        if (tvScreen == null) return;
		VideoPlayer player = tvScreen.GetComponent<VideoPlayer>();
        on = !on;
	}

	public bool CheckUnlocked()
	{
        return true;
	}

	public void Interact()
	{
		Toggle();
	}

	public bool On { get => on; }
    public float Volume { get => tvScreen.GetComponent<VideoPlayer>().GetDirectAudioVolume(0); set => tvScreen.GetComponent<VideoPlayer>().SetDirectAudioVolume(0, value);}
    public bool Mute { get => tvScreen.GetComponent<VideoPlayer>().GetDirectAudioMute(0); set => tvScreen.GetComponent<VideoPlayer>().SetDirectAudioMute(0, value); }

}
