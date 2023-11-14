using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour, Iinteract
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;

	[SerializeField] bool open = false;
    
    public bool Check()
    {
        return gameManager.instance.unlockedDoors;
    }
    public void Interact()
    {
        open = !open;
        anim.SetBool("isOpen", open);
        aud.Play();
    }
}
