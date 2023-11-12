using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class gold : MonoBehaviour, iInteract
{
    [SerializeField] int amount;
    public bool checkLock()
    {
        return true;
    }
    public void interact()
    {
        gameManager.instance.addTempGold(amount);
        gameManager.instance.collectCoins();
        Destroy(gameObject);
    }
}
