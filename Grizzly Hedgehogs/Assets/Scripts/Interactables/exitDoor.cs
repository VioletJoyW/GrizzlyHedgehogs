using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour, Iinteract
{
    public bool Check()
    {
        return true;
    }

    public void Interact()
    {
        gameManager.instance.showConfirmExitMenu();
    }
}
