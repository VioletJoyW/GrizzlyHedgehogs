using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{
    public void firstStart(Button button)
    {
        gameManager.instance.startRun();
        button.gameObject.SetActive(false);
        gameManager.instance.PlayButtonPress();
    }
    public void resume()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.PlayButtonPress();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnPause();
        gameManager.instance.PlayButtonPress();
    }
    public void mainMenu()
    {
        gameManager.instance.showMainMenu();
        gameManager.instance.PlayButtonPress();
    }

    public void controls()
    {
        gameManager.instance.showControls();
        gameManager.instance.PlayButtonPress();
    }

    public void controlsNext()
    {
        gameManager.instance.switchControlsPage(1);
        gameManager.instance.PlayButtonPress();
    }
    public void controlsPrev()
    {
        gameManager.instance.switchControlsPage(-1);
        gameManager.instance.PlayButtonPress();
    }

    public void settings()
    {
        gameManager.instance.showSettings();
        gameManager.instance.PlayButtonPress();
    }

    public void settingsPage(int page)
    {
        gameManager.instance.switchSettingsPage(page);
        gameManager.instance.PlayButtonPress();
    }

    public void credits()
    {
        gameManager.instance.showCredits();
        gameManager.instance.PlayButtonPress();
    }

    public void quit()
    {
        Application.Quit();
        gameManager.instance.PlayButtonPress();
    }

    public void back()
    {
        gameManager.instance.goBack();
        gameManager.instance.PlayButtonPress();
    }
    public void backToMain()
    {
        gameManager.instance.showMain();
        gameManager.instance.PlayButtonPress();
    }

    public void startRun()
    {
        gameManager.instance.startRun();
        gameManager.instance.PlayButtonPress();
    }

    public void endRun()
    {
        gameManager.instance.exitToInventory();
        gameManager.instance.PlayButtonPress();
    }

}
