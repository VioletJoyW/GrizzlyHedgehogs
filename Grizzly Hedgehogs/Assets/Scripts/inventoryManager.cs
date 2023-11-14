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

    [SerializeField] ScriptableGunStats rifle;
    [SerializeField] ScriptableGunStats shotgun;
    [SerializeField] ScriptableGunStats four;

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

    public void updatePlayerArmor(ScriptableArmorStats selectedArmor)
    {
        gameManager.instance.playerScript.ChangeArmor(selectedArmor);
    }

    public void unlockArmorAgile(Button button)
    {
        if (gameManager.instance.GetTotalGold() >= costArmorAgile)
        {
            gameManager.instance.addTotalGold(-costArmorAgile);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);
        }
    }

    public void unlockArmorResilient(Button button)
    {
        if (gameManager.instance.GetTotalGold() >= costArmorResilient)
        {
            gameManager.instance.addTotalGold(-costArmorResilient);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);
        }
    }

    public void unlockArmorElite(Button button)
    {
        if (gameManager.instance.GetTotalGold() >= costArmorElite)
        {
            gameManager.instance.addTotalGold(-costArmorElite);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);
        }
    }

    public void unlockGunRifle(Button button) 
    {
        if (gameManager.instance.GetTotalGold() >= costGunRifle)
        {
            gameManager.instance.addTotalGold(-costGunRifle);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);

            gameManager.instance.playerScript.AddGun(rifle);
        }
    }

    public void unlockGunShotgun(Button button)
    {
        if (gameManager.instance.GetTotalGold() >= costGunShotgun)
        {
            gameManager.instance.addTotalGold(-costGunShotgun);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);

            gameManager.instance.playerScript.AddGun(shotgun);
        }
    }

    public void unlockGunLegendary(Button button)
    {
        if (gameManager.instance.GetTotalGold() >= costGunLegendary)
        {
            gameManager.instance.addTotalGold(-costGunLegendary);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);

            gameManager.instance.playerScript.AddGun(four);
        }
    }

    public void unlockItemHealth(Button button)
    {
        if (gameManager.instance.GetTotalGold() >= costItemHealth)
        {
            gameManager.instance.addTotalGold(-costItemHealth);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);

            gameManager.instance.unlockedHealthKits = true;
        }
    }

    public void unlockItemAmmo(Button button)
    {
        if (gameManager.instance.GetTotalGold() >= costItemAmmo)
        {
            gameManager.instance.addTotalGold(-costItemAmmo);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);

            gameManager.instance.unlockedAmmoKits = true;
        }
    }

    public void unlockItemDoor(Button button)
    {
        if (gameManager.instance.GetTotalGold() >= costItemDoor)
        {
            gameManager.instance.addTotalGold(-costItemDoor);

            button.gameObject.SetActive(false);
            gameManager.instance.PlaySound(sound);

            gameManager.instance.unlockedDoors = true;
        }
    }

}
