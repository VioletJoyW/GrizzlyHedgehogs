using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TV : MonoBehaviour, Iinteract
{

    [SerializeField] GameObject tvScreen;
    [SerializeField] VideoClip video;
    [SerializeField] bool on = true;
    bool lastState;
    bool stateChanged;

	private void Awake()
	{
        lastState = on;
	}

	// Start is called before the first frame update
	void Start()
    {
        if (tvScreen == null) return;
        if (video != null) tvScreen.GetComponent<VideoPlayer>().clip = video;
        tvScreen.SetActive(on);
    }

    // Update is called once per frame
    void Update()
    {
		if (tvScreen == null) return;
		if (stateChanged)
        {
            tvScreen.SetActive(on);
            stateChanged = false;
            lastState = on;
        }
        stateChanged = lastState != on;
	}

    public void Toggle()
    {
        if (tvScreen == null) return;
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
