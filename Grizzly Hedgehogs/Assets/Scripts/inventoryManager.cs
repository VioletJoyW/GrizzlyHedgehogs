using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class inventoryManager : MonoBehaviour
{
    [SerializeField] int costArmorAgile;
    [SerializeField] int costArmorResilient;
    [SerializeField] int costArmorElite;
    [SerializeField] int costGunRifle;
    [SerializeField] int costGunShotgun;
    [SerializeField] int costGunLegendary;
    [SerializeField] int costItemHealth;
    [SerializeField] int costItemAmmo;
    [SerializeField] int costItemDoor;

    [SerializeField] Button selectedArmorButton;

    [SerializeField] scriptableGunStats rifle;
    [SerializeField] scriptableGunStats shotgun;
    [SerializeField] scriptableGunStats four;

    [SerializeField] AudioClip sound;

    void Start()
    {

    }

    public void armorSelection(Button selectedButton)
    {
        selectedArmorButton.image.color = Color.gray;
        selectedArmorButton = selectedButton;
        selectedArmorButton.image.color = Color.white;
    }

    public void updatePlayerArmor(scriptableArmorStats selectedArmor)
    {
        gameManager.instance.playerScript.changeArmor(selectedArmor);
    }

    public void unlockArmorAgile(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costArmorAgile)
        {
            gameManager.instance.addTotalGold(-costArmorAgile);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);
        }
    }

    public void unlockArmorResilient(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costArmorResilient)
        {
            gameManager.instance.addTotalGold(-costArmorResilient);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);
        }
    }

    public void unlockArmorElite(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costArmorElite)
        {
            gameManager.instance.addTotalGold(-costArmorElite);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);
        }
    }

    public void unlockGunRifle(Button button) 
    {
        if (gameManager.instance.getTotalGold() >= costGunRifle)
        {
            gameManager.instance.addTotalGold(-costGunRifle);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);

            gameManager.instance.playerScript.addGun(rifle);
        }
    }

    public void unlockGunShotgun(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costGunShotgun)
        {
            gameManager.instance.addTotalGold(-costGunShotgun);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);

            gameManager.instance.playerScript.addGun(shotgun);
        }
    }

    public void unlockGunLegendary(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costGunLegendary)
        {
            gameManager.instance.addTotalGold(-costGunLegendary);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);

            gameManager.instance.playerScript.addGun(four);
        }
    }

    public void unlockItemHealth(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costItemHealth)
        {
            gameManager.instance.addTotalGold(-costItemHealth);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);

            gameManager.instance.unlockedHealthKits = true;
        }
    }

    public void unlockItemAmmo(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costItemAmmo)
        {
            gameManager.instance.addTotalGold(-costItemAmmo);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);

            gameManager.instance.unlockedAmmoKits = true;
        }
    }

    public void unlockItemDoor(Button button)
    {
        if (gameManager.instance.getTotalGold() >= costItemDoor)
        {
            gameManager.instance.addTotalGold(-costItemDoor);

            button.gameObject.SetActive(false);
            gameManager.instance.playSound(sound);

            gameManager.instance.unlockedDoors = true;
        }
    }

}
