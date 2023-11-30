using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class key : MonoBehaviour, Iitem
{
    [SerializeField] AudioClip sound;


    int inventoryID;

	public bool CheckUnlocked()
    {
        return true;
    }
    public void Interact()
    {
        gameManager.instance.playerScript.AddItemToInventory(this);
        gameManager.instance.PlaySound(sound, settingsManager.sm.objectVol);
        Destroy(gameObject);
    }

	public int ID { get => inventoryID; set => inventoryID = value; }
}
