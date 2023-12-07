using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings")]

public class ScriptableSettings : ScriptableObject
{
    public bool varsSet = false;

    public float camSensitivity;
    public bool invertY;
    public bool camBob;
    public float textSize;

    public float globalVol;
    public float playerVol;
    public float enemyVol;
    public float objectVol;
    public float musicVol;

    public KeyCode forwards;
    public KeyCode backwards;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode crouch;
    public KeyCode powerBtnToggle;
    public KeyCode powerBtnScrollUp;
    public KeyCode powerBtnScrollDown;
    public KeyCode interact;
    public KeyCode shoot;
    public KeyCode reload;
    public KeyCode aim;
}
