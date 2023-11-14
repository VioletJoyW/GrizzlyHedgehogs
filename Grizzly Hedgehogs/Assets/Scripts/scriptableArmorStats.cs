using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Armor Stats")]

public class ScriptableArmorStats : ScriptableObject
{
    public int healthMax;
    public float staminaMax;
    public float speed;
    public float sprintSpeed;
    public float jumpHeight;
    public float restoreStaminaRate;
}
