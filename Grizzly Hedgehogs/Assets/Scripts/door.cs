using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour, iInteract
{
    [SerializeField] Animator anim;

	[SerializeField] bool open = false;
    
    public bool checkLock()
    {
        return gameManager.instance.unlockedDoors;
    }
    public void interact()
    {
        open = !open;
        anim.SetBool("isOpen", open);
    }
}
