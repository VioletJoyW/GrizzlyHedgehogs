using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunInventory : MonoBehaviour
{
    //This is the Gun Inventory System!
    //the starter gun will be unlocked at "Stage 0"
    //once you unlock the next guns then you will be
    //able to switch guns using the mouse wheel 
    //(Currently disabled for testing) + (the testing button to switch is the Right Shift)
    //Guns are set in rarety, as follows:
    // Gun -> Water -> Fire -> Electric -> Plasma
    //   0 ->     1 ->    2 ->        3 ->      4  (these are the array numbers if you wanted to change anything)


    private int currentGunIndex = 0;

    // Array to store references to each gun GameObject
    private GameObject[] guns;

    // Prefab for each gun
    public GameObject[] gunPrefabs;

    // Reference to the gun position GameObject
    [SerializeField] private GameObject gunPos;

    // Array to store whether each gun is unlocked
    private bool[] gunUnlocked;

    void Start()
    {
        guns = new GameObject[gunPrefabs.Length];
        gunUnlocked = new bool[gunPrefabs.Length];

        // Unlock the first gun and lock all others
        for (int i = 0; i < gunUnlocked.Length; i++)
        {
            gunUnlocked[i] = (i == 0); // Unlock the first gun, lock all others
        }

        // Instantiate and set up each gun
        for (int i = 0; i < gunPrefabs.Length; i++)
        {
            GameObject gunInstance = Instantiate(gunPrefabs[i]);
            gunInstance.transform.SetParent(transform); // Set the gun as a child of the camera (only for organization)
            gunInstance.SetActive(false); // Deactivate all guns initially (no one wants to see all the guns at once... unless)

            guns[i] = gunInstance;
        }

        ActivateGun(currentGunIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            SwitchGun();
        }
    }

    void SwitchGun()
    {
        guns[currentGunIndex].SetActive(false);

        currentGunIndex = (currentGunIndex + 1) % guns.Length;

        while (!gunUnlocked[currentGunIndex])
        {
            currentGunIndex = (currentGunIndex + 1) % guns.Length;
        }

        ActivateGun(currentGunIndex);
    }

    void ActivateGun(int index)
    {
        // Activate the gun if it's unlocked
        if (gunUnlocked[index])
        {
            guns[index].SetActive(true);
            guns[index].transform.position = gunPos.transform.position;
        }
        else
        {
            SwitchGun();
        }
    }
}
