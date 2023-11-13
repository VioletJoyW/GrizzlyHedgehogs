using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class gold : MonoBehaviour, iInteract
{
    [SerializeField] int amount;
    [SerializeField] AudioClip sound;
    public bool checkLock()
    {
        return true;
    }
    public void interact()
    {
        gameManager.instance.addTempGold(amount);
        gameManager.instance.playSound(sound);
        Destroy(gameObject);
    }

}
