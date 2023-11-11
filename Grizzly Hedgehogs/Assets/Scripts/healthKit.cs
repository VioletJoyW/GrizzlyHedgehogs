using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthKit : MonoBehaviour, iInteract   
{
    [SerializeField] int healAmount;
    [SerializeField] Animator animator;
    public bool checkLock()
    {
        return gameManager.instance.unlockedHealthKits;
    }
    public void interact()
    {
        animator.SetTrigger("isOpen");
        gameManager.instance.playerScript.addHealth(healAmount);

    }
    public void destroySelf()
    {
        Destroy(gameObject);
    }
}
