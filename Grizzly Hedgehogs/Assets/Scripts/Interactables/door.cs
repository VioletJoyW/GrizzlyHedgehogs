using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour, Iinteract
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;

	[SerializeField] bool open = false;
    [SerializeField] bool isUnlocked;

    [SerializeField] int keyID;

    public bool CheckUnlocked()
    {
        return isUnlocked;
    }
    public void Interact()
    {
        anim.SetBool("isOpen", open);
        aud.volume = settingsManager.sm.settingsCurr.objectVol;
        aud.Play();
        for (int i = 0; i < InventoryManager.Inventory.Count && !isUnlocked; i++)
        {
            // if null then continue
            if (InventoryManager.Inventory[i] == null)
            {
                continue;
            }
            // compares keyid
            if (InventoryManager.Inventory[i].ID == keyID)
            {
                isUnlocked = true;
                InventoryManager.RemoveItem(InventoryManager.Inventory[i].ID);
            }
        }
        if (isUnlocked)
        {
            open = !open;
        }
    }
}
