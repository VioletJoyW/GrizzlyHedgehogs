using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class settingsManager : MonoBehaviour
{
    public static settingsManager sm;

    [Header("_-_-_- Labels -_-_-_")]
    [SerializeField] TMP_Text sensitivityValue;
    [SerializeField] TMP_Text globalVolValue;
    [SerializeField] TMP_Text playerVolValue;
    [SerializeField] TMP_Text enemyVolValue;
    [SerializeField] TMP_Text enviromentVolValue;
    [SerializeField] TMP_Text musicVolValue;

    [Header("_-_-_- Defaults -_-_-_")]
    [SerializeField] float camSensitivityDefault;
    [SerializeField] bool invertYDefault;
    [SerializeField] bool camBobDefault;

    [SerializeField] float globalVolDefault;
    [SerializeField] float playerVolDefault;
    [SerializeField] float enemyVolDefault;
    [SerializeField] float enviromentVolDefault;
    [SerializeField] float musicVolDefault;

    [SerializeField] KeyCode forwardsDefault;
    [SerializeField] KeyCode backwardsDefault;
    [SerializeField] KeyCode leftDefault;
    [SerializeField] KeyCode rightDefault;
    [SerializeField] KeyCode jumpDefault;
    [SerializeField] KeyCode sprintDefault;
    [SerializeField] KeyCode interactDefault;
    [SerializeField] KeyCode shootDefault;
    [SerializeField] KeyCode reloadDefault;


    [Header("_-_-_- Current -_-_-_")]
    public float camSensitivity;
    public bool invertY;
    public bool camBob;

    public float globalVol;
    public float playerVol;
    public float enemyVol;
    public float enviromentVol;
    public float musicVol;

    public KeyCode forwards;
    public KeyCode backwards;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode interact;
    public KeyCode shoot;
    public KeyCode reload;

    Event keyEvent;
    bool waitingForKey;
    KeyCode newKey;

    void Start()
    {
        resetCamera();
        resetAudio();
        resetControls();
        waitingForKey = false;
    }

    private void OnGUI()
    {
        keyEvent = Event.current;
    }

    public void resetCamera()
    {
        camSensitivity = camSensitivityDefault;
        invertY = invertYDefault;
        camBob = camBobDefault;
    }

    public void resetAudio()
    {
        globalVol = globalVolDefault;
        playerVol = playerVolDefault;
        enemyVol = enemyVolDefault;
        enviromentVol = enviromentVolDefault;
        musicVol = musicVolDefault;
    }

    public void resetControls()
    {
        forwards = forwardsDefault;
        backwards = backwardsDefault;
        left = leftDefault;
        right = rightDefault;
        jump = jumpDefault;
        sprint = sprintDefault;
        interact = interactDefault;
        shoot = shootDefault;
        reload = reloadDefault;
    }

    public void changeCamSensitivity(float sensitivity)
    {
        camSensitivity = sensitivity;
        sensitivityValue.text = sensitivity.ToString();
    }

    public void changeInvertY(bool state)
    {
        invertY = state;
    }

    public void changeCamBob(bool state)
    {
        camBob = state; 
    }

    public void changeGlobalVol(float vol)
    {
        globalVol = vol;
        globalVolValue.text = vol.ToString();
    }
    public void changePlayerVol(float vol)
    {
        playerVol = vol;
        playerVolValue.text = vol.ToString();
    }
    public void changeEnemyVol(float vol)
    {
        enemyVol = vol;
        enemyVolValue.text = vol.ToString();
    }
    public void changeEnviromentVol(float vol)
    {
        enviromentVol = vol;
        enviromentVolValue.text = vol.ToString();
    }
    public void changeMusicVol(float vol)
    {
        musicVol = vol;
        musicVolValue.text = vol.ToString();
    }

}
