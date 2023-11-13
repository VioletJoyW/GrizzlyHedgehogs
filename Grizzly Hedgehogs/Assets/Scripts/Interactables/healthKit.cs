using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthKit : MonoBehaviour, iInteract   
{
    [SerializeField] int healAmount;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource aud;
    public bool checkLock()
    {
        return gameManager.instance.unlockedHealthKits;
    }
    public void interact()
    {
        animator.SetTrigger("isOpen");
        gameManager.instance.playerScript.addHealth(healAmount);

        aud.Play();
    }
    public void destroySelf()
    {
        Destroy(gameObject);
    }
}
