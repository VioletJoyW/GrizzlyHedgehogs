using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        for (int i = 0; !isUnlocked && i < InventoryManager.Inventory.Count; i++)
        {
            // if null then continue
            if (InventoryManager.Inventory[i] == null) continue;
            
            // compares keyid
            if (InventoryManager.Inventory[i].ID == keyID)
            {
                isUnlocked = true;
                InventoryManager.RemoveItem(InventoryManager.Inventory[i].Inventory_ID);
            }
        }
        if (isUnlocked) open = !open;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if (!open)
            {
                open = true;
            }
        }
    }
}
