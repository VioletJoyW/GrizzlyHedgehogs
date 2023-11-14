using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthKit : MonoBehaviour, Iinteract   
{
    [SerializeField] int healAmount;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource aud;
    public bool Check()
    {
        return gameManager.instance.unlockedHealthKits;
    }
    public void Interact()
    {
        animator.SetTrigger("isOpen");
        gameManager.instance.playerScript.AddHealth(healAmount);

        aud.Play();
    }

    /// <summary>
    /// Removes the health kit object.
    /// </summary>
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
