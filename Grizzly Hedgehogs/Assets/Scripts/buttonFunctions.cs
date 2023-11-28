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
    }
    public void resume()
    {
        gameManager.instance.stateUnPause();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnPause();
    }
    public void mainMenu()
    {
        gameManager.instance.showMainMenu();
    }

    public void controls()
    {
        gameManager.instance.showControls();
    }

    public void controlsNext()
    {
        gameManager.instance.switchControlsPage(1);
    }
    public void controlsPrev()
    {
        gameManager.instance.switchControlsPage(-1);
    }

    public void settings()
    {
        gameManager.instance.showSettings();
    }

    public void settingsPage(int page)
    {
        gameManager.instance.switchSettingsPage(page);
    }

    public void credits()
    {
        gameManager.instance.showCredits();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void back()
    {
        gameManager.instance.goBack();
    }
    public void backToMain()
    {
        gameManager.instance.showMain();
    }

    public void startRun()
    {
        gameManager.instance.startRun();
    }

    public void endRun()
    {
        gameManager.instance.exitToInventory();
    }

}
