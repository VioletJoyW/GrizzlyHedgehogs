using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthKit : MonoBehaviour, iInteract   
{
    [SerializeField] int healAmount;
    [SerializeField] Animator animator;
    [SerializeField] Collider box;

    public void interact()
    {
        animator.SetBool("isOpen", true);
        gameManager.instance.playerScript.changeHealth(healAmount);
        box.enabled = false;
    }
}
