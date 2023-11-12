using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun Stats")]
public class scriptableGunStats : ScriptableObject
{
    public int shootDamage;
    public int shootDistance;
    public float shootRate;
    public int ammoCurrent;
    public int ammoMax;

    public GameObject model;
    public ParticleSystem hitEffect;

    public AudioClip shootSound;
    [Range(0, 1)] public float shootSoundVol;

    public AudioClip emptySound;
    [Range(0, 1)] public float emptySoundVol;
}
