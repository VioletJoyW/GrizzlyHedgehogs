using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthKit : MonoBehaviour, Iinteract   
{
    [SerializeField] int healAmount;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource aud;


    public bool CheckUnlocked()
    {
        return true;
    }
    public void Interact()
    {
        animator.SetTrigger("isOpen");
        gameManager.instance.playerScript.AddHealth(healAmount);

        aud.volume = settingsManager.sm.settingsCurr.objectVol;
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
