using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class inventoryManager : MonoBehaviour
{
    [SerializeField] Button[] armorChoices;
    int armorChosen = 0;
    [SerializeField] Button[] locks = new Button[10];
    [SerializeField] int[] costs = new int[10];
    void Start()
    {

    }

    public void armorSelection(int slot)
    {
        if (slot > armorChoices.Length || slot < 0)
        {
            return;
        }
        for (int i = 0; i <= armorChoices.Length; i++)
        {
            armorChoices[i].image.color = Color.gray;
        }
        armorChoices[slot].image.color = Color.white;
        armorChosen = slot;
    }

    public void unlock(int slot)
    {
        if (slot > locks.Length || slot < 0)
        {
            return;
        }
        if(gameManager.instance.getTotalGold() >= costs[slot])
        {
            locks[slot].gameObject.SetActive(false);
            gameManager.instance.addTotalGold(-costs[slot]);

            switch(slot)
            {
                case 3:
                    //Add Rifle
                    break;
                case 4:
                    //Add Shotgun
                    break;
                case 5:
                    //Add Legendary
                    break;
                case 6:
                    gameManager.instance.unlockedHealthKits = true;
                    break;
                case 7:
                    gameManager.instance.unlockedAmmoKits = true;
                    break;
                case 8:
                    gameManager.instance.unlockedDoors = true;
                    break;
                case 9:
                    //TEMP
                    break;
            }
        }
    }
}
