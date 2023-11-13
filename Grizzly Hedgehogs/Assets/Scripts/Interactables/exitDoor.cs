using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour, iInteract
{
    public bool checkLock()
    {
        return true;
    }

    public void interact()
    {
        gameManager.instance.showConfirmExitMenu();
    }
}
