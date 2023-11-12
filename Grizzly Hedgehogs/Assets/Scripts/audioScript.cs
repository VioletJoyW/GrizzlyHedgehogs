using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioScript : MonoBehaviour
{
    new public AudioSource audio;
    [SerializeField] AudioClip heal, coins, footsteps, gunshot;

    
    public void healPlayer()
    {
        audio.clip = heal;
        audio.Play();
    }

    public void collectCoins()
    {
        audio.clip = coins;
        audio.Play();
    }

    //public void footSteps()
    //{
    //    audio.clip = footsteps;
    //    audio.Play();
    //}

    public void gunShot()
    {
        audio.clip = gunshot;
        audio.Play();
    }
}
