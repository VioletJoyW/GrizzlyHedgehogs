using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[ExecuteInEditMode]

public class key : MonoBehaviour, Iitem, Iinteract
{
    [SerializeField] AudioClip sound;
    [SerializeField] int keyID;

    int inventoryID;

    void Start()
    {
        keyID = ID;
    }

	public bool CheckUnlocked()
    {
        return true;
    }
    public void Interact()
    {
        gameManager.instance.playerScript.AddItemToInventory(this);
        gameManager.instance.PlaySound(sound, settingsManager.sm.settingsCurr.objectVol);
        Destroy(gameObject);
    }
	public int ID { get => inventoryID; set => inventoryID = value; }
}
