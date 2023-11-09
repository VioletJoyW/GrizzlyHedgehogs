using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoKit : MonoBehaviour, iInteract   
{
    [SerializeField] int ammoAmount;
    [SerializeField] Animator animator;
    [SerializeField] Collider box;

    public void interact()
    {
        animator.SetBool("isOpen", true);
        gameManager.instance.playerScript.changeAmmo(ammoAmount);
        box.enabled = false;
    }
}
