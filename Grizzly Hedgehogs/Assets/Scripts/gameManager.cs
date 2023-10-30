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
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public GameObject player;

    public bool isPaused;
    float timescaleOrig;
    int enemiesRemaining;
    GameObject menuPrevious;

    void Awake()
    {
        instance = this;
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            showPauseMenu();
        }

        //CODE FOR TESTING - DELETE LATER
        if(Input.GetKeyDown(KeyCode.Y))
        {
            youWin();
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
        statePause();
        menuActive = menuPause;
        menuActive.SetActive(true);
    }

    public void showControlsMenu()
    {
        menuActive.SetActive(false);
        menuPrevious = menuActive;
        menuActive = menuControls;
        menuActive.SetActive(true);
    }
    public void showCreditsMenu()
    {
        menuActive.SetActive(false);
        menuPrevious = menuActive;
        menuActive = menuCredits;
        menuActive.SetActive(true);
    }

    public void youWin()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }
    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void goBack()
    {
        menuActive.SetActive(false);
        menuActive = menuPrevious;
        menuActive.SetActive(true);
    }

    public void updateGameGoal(int amount)
    {
        // Checking how many enemies remain
        enemiesRemaining += amount;

        if (enemiesRemaining <= 0)
        {
            youWin();
        }
    }
}
