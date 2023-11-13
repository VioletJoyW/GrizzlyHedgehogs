using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoKit : MonoBehaviour, iInteract   
{
    [SerializeField] int ammoAmount;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource aud;
    public bool checkLock()
    {
        return gameManager.instance.unlockedAmmoKits;
    }
    public void interact()
    {
        animator.SetTrigger("isOpen");
        gameManager.instance.playerScript.addAmmo(ammoAmount);
        aud.Play();
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }

}
