using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour, Iinteract
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;

	[SerializeField] bool open = false;
    
    public bool CheckUnlocked()
    {
        return gameManager.instance.unlockedDoors;
    }
    public void Interact()
    {
        open = !open;
        anim.SetBool("isOpen", open);
        aud.volume = settingsManager.sm.objectVol;
        aud.Play();
    }
}
