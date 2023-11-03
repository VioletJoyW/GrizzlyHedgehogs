using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healItem : MonoBehaviour, iInteract   
{
    [SerializeField] int healAmount;
    public void interact()
    {
        gameManager.instance.playerScript.changeHealth(healAmount);
    }
}
