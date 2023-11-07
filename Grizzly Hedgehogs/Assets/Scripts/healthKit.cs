using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthKit : MonoBehaviour, iInteract   
{
    [SerializeField] int healAmount;
    [SerializeField] Animator animator;
    [SerializeField] Collider box;

    private void Start()
    {
        animator.enabled = false;
    }
    public void interact()
    {
        animator.enabled = true;
        gameManager.instance.playerScript.changeHealth(healAmount);
        box.enabled = false;
    }
}
