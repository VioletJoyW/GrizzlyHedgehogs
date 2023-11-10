using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoKit : MonoBehaviour, iInteract   
{
    [SerializeField] int ammoAmount;
    [SerializeField] Animator animator;
    public bool checkLock()
    {
        return gameManager.instance.unlockedAmmoKits;
    }
    public void interact()
    {
        animator.SetTrigger("isOpen");
        gameManager.instance.playerScript.changeAmmo(ammoAmount);
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }

}
