using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oracleOrb : MonoBehaviour, Iinteract
{
    public ParticleSystem iEffect;
    public AudioClip iSound;
    public AudioSource aud;
    [Range(0, 1)] public float iSoundVol;

    bool levelDone = false;

    // Start is called before the first frame update
    void Start()
    {
        
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

        //TODO: Check if Level Win conditions are met

        if (levelDone)
        {
            //Win dialog & take player to the next level
        }
        else
        {
            //Tell player to finish the level first
        }
    }
}