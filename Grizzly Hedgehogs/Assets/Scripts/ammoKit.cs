using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoKit : MonoBehaviour, iInteract   
{
    [SerializeField] int ammoAmount;
    [SerializeField] Animator animator;
    [SerializeField] Collider box;

    private void Start()
    {
        animator.enabled = false;
    }
    public void interact()
    {
        animator.enabled = true;
        gameManager.instance.playerScript.changeAmmo(ammoAmount);
        box.enabled = false;
    }
}
