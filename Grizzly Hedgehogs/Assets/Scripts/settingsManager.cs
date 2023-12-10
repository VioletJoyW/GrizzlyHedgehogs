using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class settingsManager : MonoBehaviour
{
    public static settingsManager sm;

    public ScriptableSettings settingsCurr;

    [Header("_-_-_- Components -_-_-_")]
    [SerializeField] TMP_Text sensitivityValue;
    [SerializeField] GameObject textSizeValue;
    [SerializeField] TMP_Text camFOV;
    [SerializeField] TMP_Text globalVolValue;
    [SerializeField] TMP_Text playerVolValue;
    [SerializeField] TMP_Text enemyVolValue;
    [SerializeField] TMP_Text enviromentVolValue;
    [SerializeField] TMP_Text musicVolValue;

    [SerializeField] Slider camSliderValue;
    [SerializeField] Slider textSliderValue;
    [SerializeField] Slider fovSlider;
    [SerializeField] Toggle invertYCheck;
    [SerializeField] Toggle camBobCheck;
    [SerializeField] Button[] Keys;
    [SerializeField] Slider[] VolValues;

    [SerializeField] AudioClip testAudio;

    [Header("_-_-_- Defaults -_-_-_")]
    [SerializeField] float camSensitivityDefault;
    [SerializeField] bool invertYDefault;
    [SerializeField] bool camBobDefault;
    [SerializeField] float textSizeDefault;

    [SerializeField] float globalVolDefault;
    [SerializeField] float playerVolDefault;
    [SerializeField] float enemyVolDefault;
    [SerializeField] float enviromentVolDefault;
    [SerializeField] float musicVolDefault;
    [SerializeField] float aimFOVDefault;


    [SerializeField] KeyCode forwardsDefault;
    [SerializeField] KeyCode backwardsDefault;
    [SerializeField] KeyCode leftDefault;
    [SerializeField] KeyCode rightDefault;
    [SerializeField] KeyCode jumpDefault;
    [SerializeField] KeyCode sprintDefault;
    [SerializeField] KeyCode powerBtnToggleDefault;
    [SerializeField] KeyCode powerBtnScrollUpDefault;
    [SerializeField] KeyCode powerBtnScrollDownDefault;
    [SerializeField] KeyCode crouchDefault;
    [SerializeField] KeyCode interactDefault;
    [SerializeField] KeyCode shootDefault;
    [SerializeField] KeyCode reloadDefault;
    [SerializeField] KeyCode aimDefault;

    Event keyEvent;
    bool waitingForKey;
    KeyCode newKey;

    bool UInoises = false;

    void Start()
    {
        sm = this;

        if(!settingsCurr.varsSet)
        {
            resetVisuals();
            resetAudio();
            resetControls();
            settingsCurr.varsSet = true;
        }
        else
        {
            showVisualsChanges();
            showAudioChanges();
            showControlsChanges();
        }

        waitingForKey = false;
        StartCoroutine(slidersMakeNoise());
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

    public void resetVisuals()
    {
        if (UInoises)
        {
            gameManager.instance.PlayButtonPress();
        }

        settingsCurr.camSensitivity = camSensitivityDefault;

        settingsCurr.invertY = invertYDefault;

        settingsCurr.camBob = camBobDefault;

        settingsCurr.textSize = textSizeDefault;

        showVisualsChanges();
    }

    public void showVisualsChanges()
    {
        gameManager.instance.ChangeTextSize();

        sensitivityValue.text = settingsCurr.camSensitivity.ToString();
        camSliderValue.value = settingsCurr.camSensitivity;
        camFOV.text = settingsCurr.camFOV.ToString();
        fovSlider.value = settingsCurr.camFOV;
        invertYCheck.isOn = settingsCurr.invertY;
        camBobCheck.isOn = settingsCurr.camBob;
        textSizeValue.GetComponentInChildren<TMP_Text>().text = settingsCurr.textSize.ToString();
        textSliderValue.value = settingsCurr.textSize;
    }

    public void resetAudio()
    {
        if (UInoises)
        {
            gameManager.instance.PlayButtonPress();
        }

        settingsCurr.globalVol = globalVolDefault;
        settingsCurr.playerVol = playerVolDefault;
        settingsCurr.enemyVol = enemyVolDefault;
        settingsCurr.objectVol = enviromentVolDefault;
        settingsCurr.musicVol = musicVolDefault;

        showAudioChanges();
    }

    public void showAudioChanges()
    {
        globalVolValue.text = settingsCurr.globalVol.ToString("F2");
        playerVolValue.text = settingsCurr.playerVol.ToString("F2");
        enemyVolValue.text = settingsCurr.enemyVol.ToString("F2");
        enviromentVolValue.text = settingsCurr.objectVol.ToString("F2");
        musicVolValue.text = settingsCurr.musicVol.ToString("F2");

        for (int i = 0; i < VolValues.Length; i++)
        {
            VolValues[i].value = 0.5f;
        }

        AudioListener.volume = settingsCurr.globalVol;
        gameManager.instance.ChangeMusicVol();
    }

    public void resetControls()
    {
        if (UInoises)
        {
            gameManager.instance.PlayButtonPress();
        }

        settingsCurr.forwards = forwardsDefault;
        settingsCurr.backwards = backwardsDefault;
        settingsCurr.left = leftDefault;
        settingsCurr.right = rightDefault;
        settingsCurr.jump = jumpDefault;
        settingsCurr.sprint = sprintDefault;
        settingsCurr.crouch = crouchDefault;
        settingsCurr.powerBtnToggle = powerBtnToggleDefault;
        settingsCurr.powerBtnScrollDown = powerBtnScrollDownDefault;
        settingsCurr.powerBtnScrollUp = powerBtnScrollUpDefault;
        settingsCurr.interact = interactDefault;
        settingsCurr.shoot = shootDefault;
        settingsCurr.reload = reloadDefault;

        settingsCurr.camFOV = aimFOVDefault;
        
        showControlsChanges();
    }

    public void showControlsChanges()
    {
        Keys[0].GetComponentInChildren<TMP_Text>().text = settingsCurr.forwards.ToString();
        Keys[1].GetComponentInChildren<TMP_Text>().text = settingsCurr.backwards.ToString();
        Keys[2].GetComponentInChildren<TMP_Text>().text = settingsCurr.left.ToString();
        Keys[3].GetComponentInChildren<TMP_Text>().text = settingsCurr.right.ToString();
        Keys[4].GetComponentInChildren<TMP_Text>().text = settingsCurr.jump.ToString();
        Keys[5].GetComponentInChildren<TMP_Text>().text = settingsCurr.sprint.ToString();
        Keys[6].GetComponentInChildren<TMP_Text>().text = settingsCurr.crouch.ToString();
        Keys[7].GetComponentInChildren<TMP_Text>().text = settingsCurr.interact.ToString();
        Keys[8].GetComponentInChildren<TMP_Text>().text = settingsCurr.shoot.ToString();
        Keys[9].GetComponentInChildren<TMP_Text>().text = settingsCurr.reload.ToString();
        Keys[10].GetComponentInChildren<TMP_Text>().text = settingsCurr.powerBtnToggle.ToString();
        Keys[11].GetComponentInChildren<TMP_Text>().text = settingsCurr.powerBtnScrollUp.ToString();
        Keys[12].GetComponentInChildren<TMP_Text>().text = settingsCurr.powerBtnScrollDown.ToString();
    }

    public void changeCamSensitivity(Slider sensitivity)
    {
        settingsCurr.camSensitivity = sensitivity.value;
        sensitivityValue.text = sensitivity.value.ToString();
    }

    public void changeCamFOV(Slider fov)
    {
        settingsCurr.camFOV = fov.value;
        camFOV.text = fov.value.ToString();
    }

    public void changeTextSize(Slider size)
    {
        settingsCurr.textSize = size.value;
        textSizeValue.GetComponentInChildren<TMP_Text>().text = size.value.ToString("F2");
        textSizeValue.transform.localScale = new Vector3(size.value, size.value, size.value);
        gameManager.instance.ChangeTextSize();
    }

    public void changeInvertY(Toggle state)
    {
        settingsCurr.invertY = state.isOn;
    }

    public void changeCamBob(Toggle state)
    {
        settingsCurr.camBob = state.isOn; 
    }

    public void changeGlobalVol(Slider vol)
    {
        settingsCurr.globalVol = vol.value;
        globalVolValue.text = vol.value.ToString("F2");
        AudioListener.volume = settingsCurr.globalVol; 
        if (UInoises)
        {
            gameManager.instance.PlaySound(testAudio, vol.value);
        }
    }
    public void changePlayerVol(Slider vol)
    {
        settingsCurr.playerVol = vol.value;
        playerVolValue.text = vol.value.ToString("F2"); 
        if (UInoises)
        {
            gameManager.instance.PlaySound(testAudio, vol.value);
        }
        gameManager.instance.playerScript.ChangePlayerVol(settingsCurr.playerVol);
    }
    public void changeEnemyVol(Slider vol)
    {
        settingsCurr.enemyVol = vol.value;
        enemyVolValue.text = vol.value.ToString("F2"); 
        if (UInoises)
        {
            gameManager.instance.PlaySound(testAudio, vol.value);
        }
    }
    public void changeEnviromentVol(Slider vol)
    {
        settingsCurr.objectVol = vol.value;
        enviromentVolValue.text = vol.value.ToString("F2"); 
        if (UInoises)
        {
            gameManager.instance.PlaySound(testAudio, vol.value);
        }
        gameManager.instance.playerScript.ChangeObjectVol(settingsCurr.objectVol);
    }
    public void changeMusicVol(Slider vol)
    {
        settingsCurr.musicVol = vol.value;
        musicVolValue.text = vol.value.ToString("F2");
        gameManager.instance.ChangeMusicVol();
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
                settingsCurr.forwards = newKey;
                break;
            case "Backwards":
                settingsCurr.backwards = newKey;
                break;
            case "Left":
                settingsCurr.left = newKey;
                break;
            case "Right":
                settingsCurr.right = newKey;
                break;
            case "Jump":
                settingsCurr.jump = newKey;
                break;
            case "Sprint":
                settingsCurr.sprint = newKey;
                break;
            case "Crouch":
                settingsCurr.crouch = newKey;
                break;
            case "Interact":
                settingsCurr.interact = newKey;
                break;
            case "Shoot":
                settingsCurr.shoot = newKey;
                break;
            case "Reload":
                settingsCurr.reload = newKey;
                break;
            case "Toggle Powers":
                settingsCurr.powerBtnToggle = newKey;
                break;
            case "Power Up":
                settingsCurr.powerBtnScrollUp = newKey;
                break;
            case "Power Down":
                settingsCurr.powerBtnScrollDown = newKey;
                break;
            case "Aim":
                settingsCurr.aim = newKey;
                break;
        }

        key.GetComponentInChildren<TMP_Text>().text = newKey.ToString();
        if(newKey.ToString().Length > 10)
        {
            key.GetComponentInChildren<TMP_Text>().fontSize = (10 - newKey.ToString().Length)*2 + 50;
        }
        else
        {
            key.GetComponentInChildren<TMP_Text>().fontSize = 50;
        }
    }

    IEnumerator slidersMakeNoise()
    {
        yield return new WaitForSeconds(0.1f);
        UInoises = true;
    }

}
