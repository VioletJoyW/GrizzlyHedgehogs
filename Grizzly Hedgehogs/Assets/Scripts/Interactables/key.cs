using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[ExecuteInEditMode]



public class key : MonoBehaviour, Iitem, Iinteract
{
    static int keyCount;

    //static List<key> keys = new List<key>();

    [SerializeField] AudioClip sound;
    [SerializeField] int keyID;

    int inventoryID;
    
    void Start()
    {
        keyID = ID = keyCount++;
	}


    void Awake()
    {
        keyCount = 0;
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
	public int ID { get => keyID; set => keyID = value; }
	public int Inventory_ID { get => inventoryID; set => inventoryID = value; }
}
