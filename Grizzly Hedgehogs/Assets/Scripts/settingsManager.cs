using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class settingsManager : MonoBehaviour
{
    public static settingsManager sm;

    [Header("_-_-_- Components -_-_-_")]
    [SerializeField] TMP_Text sensitivityValue;
    [SerializeField] TMP_Text globalVolValue;
    [SerializeField] TMP_Text playerVolValue;
    [SerializeField] TMP_Text enemyVolValue;
    [SerializeField] TMP_Text enviromentVolValue;
    [SerializeField] TMP_Text musicVolValue;

    [SerializeField] Slider camSliderValue;
    [SerializeField] Toggle invertYCheck;
    [SerializeField] Toggle camBobCheck;
    [SerializeField] Button[] Keys;
    [SerializeField] Slider[] VolValues;

    [SerializeField] AudioClip testAudio;

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
    [SerializeField] KeyCode powerBtnScrollUpDefault;
    [SerializeField] KeyCode powerBtnScrollDownDefault;
    [SerializeField] KeyCode crouchDefault;
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
    public float objectVol;
    public float musicVol;

    public KeyCode forwards;
    public KeyCode backwards;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode crouch;
    public KeyCode powerBtnScrollUp;
    public KeyCode powerBtnScrollDown;
    public KeyCode interact;
    public KeyCode shoot;
    public KeyCode reload;

    Event keyEvent;
    bool waitingForKey;
    KeyCode newKey;

    void Start()
    {
        sm = this;
        resetCamera();
        resetAudio();
        resetControls();
        waitingForKey = false;
    }

    private void OnGUI()
    {
        keyEvent = Event.current;
        if(keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }

    public void resetCamera()
    {
        gameManager.instance.PlayButtonPress();

        camSensitivity = camSensitivityDefault;
        sensitivityValue.text = camSensitivity.ToString();
        camSliderValue.value = camSensitivity;

        invertY = invertYDefault;
        invertYCheck.isOn = invertY;

        camBob = camBobDefault;
        camBobCheck.isOn = camBob;
    }

    public void resetAudio()
    {
        gameManager.instance.PlayButtonPress();

        globalVol = globalVolDefault;
        globalVolValue.text = globalVol.ToString("F2");
        playerVol = playerVolDefault;
        playerVolValue.text = playerVol.ToString("F2");
        enemyVol = enemyVolDefault;
        enemyVolValue.text = enemyVol.ToString("F2");
        objectVol = enviromentVolDefault;
        enviromentVolValue.text = objectVol.ToString("F2");
        musicVol = musicVolDefault;
        musicVolValue.text = musicVol.ToString("F2");

        for(int i = 0; i < VolValues.Length; i++)
        {
            VolValues[i].value = 0.5f;
        }

        AudioListener.volume = globalVol;
    }

    public void resetControls()
    {
        gameManager.instance.PlayButtonPress();

        forwards = forwardsDefault;
        backwards = backwardsDefault;
        left = leftDefault;
        right = rightDefault;
        jump = jumpDefault;
        sprint = sprintDefault;
        crouch = crouchDefault;
        powerBtnScrollDown = powerBtnScrollDownDefault;
        powerBtnScrollUp = powerBtnScrollUpDefault;
        interact = interactDefault;
        shoot = shootDefault;
        reload = reloadDefault;
        
        Keys[0].GetComponentInChildren<TMP_Text>().text = forwards.ToString();
        Keys[1].GetComponentInChildren<TMP_Text>().text = backwards.ToString();
        Keys[2].GetComponentInChildren<TMP_Text>().text = left.ToString();
        Keys[3].GetComponentInChildren<TMP_Text>().text = right.ToString();
        Keys[4].GetComponentInChildren<TMP_Text>().text = jump.ToString();
        Keys[5].GetComponentInChildren<TMP_Text>().text = sprint.ToString();
        Keys[6].GetComponentInChildren<TMP_Text>().text = crouch.ToString();
        Keys[7].GetComponentInChildren<TMP_Text>().text = interact.ToString();
        Keys[8].GetComponentInChildren <TMP_Text>().text = shoot.ToString();
        Keys[9].GetComponentInChildren<TMP_Text>().text = reload.ToString();
    }

    public void changeCamSensitivity(Slider sensitivity)
    {
        camSensitivity = sensitivity.value;
        sensitivityValue.text = sensitivity.value.ToString();
    }

    public void changeInvertY(Toggle state)
    {
        invertY = state.isOn;
    }

    public void changeCamBob(Toggle state)
    {
        camBob = state.isOn; 
    }

    public void changeGlobalVol(Slider vol)
    {
        globalVol = vol.value;
        globalVolValue.text = vol.value.ToString("F2");
        AudioListener.volume = globalVol;
        gameManager.instance.PlaySound(testAudio, vol.value);
    }
    public void changePlayerVol(Slider vol)
    {
        playerVol = vol.value;
        playerVolValue.text = vol.value.ToString("F2");
        gameManager.instance.PlaySound(testAudio, vol.value);
        gameManager.instance.playerScript.ChangePlayerVol(playerVol);
    }
    public void changeEnemyVol(Slider vol)
    {
        enemyVol = vol.value;
        enemyVolValue.text = vol.value.ToString("F2");
        gameManager.instance.PlaySound(testAudio, vol.value);
    }
    public void changeEnviromentVol(Slider vol)
    {
        objectVol = vol.value;
        enviromentVolValue.text = vol.value.ToString("F2");
        gameManager.instance.PlaySound(testAudio, vol.value);
        gameManager.instance.playerScript.ChangeObjectVol(objectVol);
    }
    public void changeMusicVol(Slider vol)
    {
        musicVol = vol.value;
        musicVolValue.text = vol.value.ToString("F2");
        gameManager.instance.PlaySound(testAudio, vol.value);
    }

    public void startKeyChange(Button key)
    {
        if(!waitingForKey)
        {
            key.GetComponentInChildren<TMP_Text>().text = "...";
            gameManager.instance.PlayButtonPress();
            StartCoroutine(AssignKey(key));
        }
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
        {
            if (Input.GetMouseButtonDown(0))
            {
                newKey = KeyCode.Mouse0;
                waitingForKey = false;
                break;
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                newKey = KeyCode.Mouse1;
                waitingForKey = false;
                break;
            }

            if (Input.GetMouseButtonDown(2))
            {
                newKey = KeyCode.Mouse2;
                waitingForKey = false;
                break;
            }

            yield return null; 
        }
    }

    IEnumerator AssignKey(Button key)
    {
        waitingForKey = true;

        yield return WaitForKey();

        switch(key.name)
        {
            case "Forwards":
                forwards = newKey;
                break;
            case "Backwards":
                backwards = newKey;
                break;
            case "Left":
                left = newKey;
                break;
            case "Right":
                right = newKey;
                break;
            case "Jump":
                jump = newKey;
                break;
            case "Sprint":
                sprint = newKey;
                break;
            case "Crouch":
                crouch = newKey;
                break;
            case "Interact":
                interact = newKey;
                break;
            case "Shoot":
                shoot = newKey;
                break;
            case "Reload":
                reload = newKey;
                break;
        }

        key.GetComponentInChildren<TMP_Text>().text = newKey.ToString();
    }
}
