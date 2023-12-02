using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class key : MonoBehaviour, Iinteract
{
    [SerializeField] int amount;
    [SerializeField] AudioClip sound;
    public bool CheckUnlocked()
    {
        return true;
    }
    public void Interact()
    {
        gameManager.instance.AddTempGold(amount);
        gameManager.instance.PlaySound(sound, settingsManager.sm.settingsCurr.objectVol);
        Destroy(gameObject);
    }

}
