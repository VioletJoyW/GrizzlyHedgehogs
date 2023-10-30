using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.stateUnPause();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnPause();
    }

    public void controls()
    {
        gameManager.instance.showControlsMenu();
    }

    public void credits()
    {
        gameManager.instance.showCreditsMenu();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void back()
    {
        gameManager.instance.goBack();
    }
}
