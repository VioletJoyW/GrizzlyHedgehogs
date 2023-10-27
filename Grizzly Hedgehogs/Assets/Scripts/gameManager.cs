using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuControls;
    [SerializeField] GameObject menuCredits;
    //[SerializeField] GameObject menuWin;
    //[SerializeField] GameObject menuLose;

    public bool isPaused;
    float timescaleOrig;

    void Awake()
    {
        instance = this;
        timescaleOrig = Time.timeScale;
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            showPauseMenu();
            statePause();
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnPause()
    {
        isPaused = false;
        Time.timeScale = timescaleOrig;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void showPauseMenu()
    {
        if(menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = menuPause;
        menuActive.SetActive(isPaused);
    }

    public void showControlsMenu()
    {
        menuActive.SetActive(false);
        menuActive = menuControls;
        menuActive.SetActive(isPaused);
    }
    public void showCreditsMenu()
    {
        menuActive.SetActive(false);
        menuActive = menuCredits;
        menuActive.SetActive(isPaused);
    }

}
