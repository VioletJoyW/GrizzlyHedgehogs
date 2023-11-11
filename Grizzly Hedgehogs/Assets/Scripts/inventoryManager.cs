using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class inventoryManager : MonoBehaviour
{
    [SerializeField] Button[] armorChoices;
    int armorChosen = 0;

    [SerializeField] int costArmorAgile;
    [SerializeField] int costArmorResilient;
    [SerializeField] int costArmorElite;
    [SerializeField] int costGunRifle;
    [SerializeField] int costGunShotgun;
    [SerializeField] int costGunLegendary;
    [SerializeField] int costItemHealth;
    [SerializeField] int costItemAmmo;
    [SerializeField] int costItemDoor;

    void Start()
    {

    }

    public void armorSelection(int slot)
    {
        if (slot > armorChoices.Length || slot < 0)
        {
            return;
        }

        armorChoices[armorChosen].image.color = Color.gray;
        armorChosen = slot;
        armorChoices[armorChosen].image.color = Color.white;

        //Code to set player's active armor stats
    }

    public void unlockArmorAgile(Button button)
    {
        if(gameManager.instance.getTotalGold() >= costArmorAgile)
        {
            gameManager.instance.addTotalGold(-costArmorAgile);

            button.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

    public void unlockArmorResilient(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costArmorResilient)
        {
            gameManager.instance.addTotalGold(-costArmorResilient);

            button.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

    public void unlockArmorElite(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costArmorElite)
        {
            gameManager.instance.addTotalGold(-costArmorElite);

            button.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

    public void unlockGunRifle(Button button) 
    {
        if (gameManager.instance.getTotalGold() >= costGunRifle)
        {
            gameManager.instance.addTotalGold(-costGunRifle);

            button.gameObject.SetActive(false);

            //Code to add gun to player's list
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

    public void unlockGunShotgun(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costGunShotgun)
        {
            gameManager.instance.addTotalGold(-costGunShotgun);

            button.gameObject.SetActive(false);

            //Code to add gun to player's list
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

    public void unlockGunLegendary(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costGunLegendary)
        {
            gameManager.instance.addTotalGold(-costGunLegendary);

            button.gameObject.SetActive(false);

            //Code to add gun to player's list
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

    public void unlockItemHealth(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costItemHealth)
        {
            gameManager.instance.addTotalGold(-costItemHealth);

            button.gameObject.SetActive(false);

            gameManager.instance.unlockedHealthKits = true;
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

    public void unlockItemAmmo(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costItemAmmo)
        {
            gameManager.instance.addTotalGold(-costItemAmmo);

            button.gameObject.SetActive(false);

            gameManager.instance.unlockedAmmoKits = true;
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

    public void unlockItemDoor(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costItemDoor)
        {
            gameManager.instance.addTotalGold(-costItemDoor);

            button.gameObject.SetActive(false);

            gameManager.instance.unlockedDoors = true;
        }
        else
        {
            StartCoroutine(gameManager.instance.showGoldMessage());
        }
    }

}
