using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class gold : MonoBehaviour, iInteract
{
    [SerializeField] int amount;
    public void interact()
    {
        gameManager.instance.changeGold(amount);
        Destroy(gameObject);
    }
}
