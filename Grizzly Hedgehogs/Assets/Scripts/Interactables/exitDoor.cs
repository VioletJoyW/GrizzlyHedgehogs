using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour, Iinteract
{
    public bool CheckUnlocked()
    {
        return true;
    }

    public void Interact()
    {
        gameManager.instance.showConfirmExitMenu();
    }
}
