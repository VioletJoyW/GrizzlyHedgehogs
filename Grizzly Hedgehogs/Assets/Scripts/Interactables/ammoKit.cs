using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoKit : MonoBehaviour, Iinteract   
{
    [SerializeField] int ammoAmount;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource aud;
    public bool CheckUnlocked()
    {
        return gameManager.instance.unlockedAmmoKits;
    }
    public void Interact()
    {
        animator.SetTrigger("isOpen");
        gameManager.instance.playerScript.AddAmmo(ammoAmount);
        aud.volume = settingsManager.sm.settingsCurr.objectVol;
        aud.Play();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
