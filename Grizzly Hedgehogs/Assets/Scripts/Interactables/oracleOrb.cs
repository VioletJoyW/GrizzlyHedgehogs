using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class oracleOrb : MonoBehaviour, Iinteract
{
    public ParticleSystem iEffect;
    public AudioClip iSound;
    public AudioSource aud;
    [Range(0, 1)] public float iSoundVol;

    bool levelDone = false;
    [SerializeField] int curScene;

    // Start is called before the first frame update
    void Start()
    {
        levelDone = false;
    }

    public bool CheckUnlocked()
    {
        return true;
    }

    public void Interact()
    {
        if (iEffect != null)
            Instantiate(iEffect, this.transform);

        if (iSound != null)
            aud.PlayOneShot(iSound, iSoundVol);

        //TODO: Check if Level Win conditions are met in each level
        switch (curScene)
        {
            case 0:
            levelDone = gameManager.instance.updateEnemyCount(0);
            break;

            case 1:
            break;

            case 2:
            levelDone = true;
            break;

            case 3:
            break;
        }
        

        if (levelDone)
        {
            //Win dialog & take player to the next level
            gameManager.instance.ShowDialog("Nice work, onto the next one!");

            //TODO: Make Sure level change works
            SceneLoaderObj.IsDown = true;
            SceneLoaderObj.Fade(1, true);
        }
        else
        {
            //Tell player to finish the level first
            gameManager.instance.ShowDialog("You've still got work to do!");
        }
    }
}
