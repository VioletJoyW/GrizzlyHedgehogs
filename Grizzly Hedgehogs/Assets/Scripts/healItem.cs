using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healItem : MonoBehaviour, iInteract   
{
    [SerializeField] int healAmount;
    [SerializeField] Animator animator;

    private void Start()
    {
        animator.enabled = false;
    }
    public void interact()
    {
        animator.enabled = true;
        gameManager.instance.playerScript.changeHealth(healAmount);
    }
}
