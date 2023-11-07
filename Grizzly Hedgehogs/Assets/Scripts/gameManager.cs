using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("_-_-_- Menus -_-_-_")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuControls;
    [SerializeField] GameObject menuCredits;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [Header("_-_-_- HUD -_-_-_")]
    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text enemyCountText;

    [SerializeField] GameObject interactPrompt;
    [SerializeField] Image playerHealthBar;
    [SerializeField] TMP_Text playerHealthText;
    [SerializeField] Image playerStaminaBar;
    [SerializeField] TMP_Text playerStaminaText;
    [SerializeField] Image playerAmmoBar;
    [SerializeField] TMP_Text playerAmmoText;

    [Header("_-_-_- Player Info -_-_-_")]
    public GameObject playerSpawnPos;
    public GameObject player;
    public playerController playerScript;

    public bool isPaused;
    float timescaleOrig;
    int enemiesRemaining;
    GameObject menuPrevious;

    void Awake()
    {
        instance = this;
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("Respawn");
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel") && !isPaused)
        {
            showPauseMenu();
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
        enemiesRemaining += amount;
        enemyCountText.text = enemiesRemaining.ToString("0");
        if (enemiesRemaining <= 0)
        {
            youWin();
        }
    }

    public void updatePlayerUI(int playerHealth, int playerHealthOrig, float playerStamina, float playerStaminaOrig, int playerAmmo, int playerAmmoOrig)
    {
        playerHealthBar.fillAmount = (float)playerHealth / playerHealthOrig;
        playerHealthText.text = playerHealth.ToString("0") + " / " + playerHealthOrig.ToString("0");
        playerStaminaBar.fillAmount = playerStamina / playerStaminaOrig;
        playerStaminaText.text = playerStamina.ToString("0") + " / " + playerStaminaOrig.ToString("0");
        playerAmmoBar.fillAmount = (float)playerAmmo / playerAmmoOrig;
        playerAmmoText.text = playerAmmo.ToString("0") + " / " + playerAmmoOrig.ToString("0");
    }

    public void showInteractPrompt(bool on)
    {
        interactPrompt.SetActive(on);
    }
    public IEnumerator playerFlashDamage()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(.1f);
        playerDamageScreen.SetActive(false);
    }
}
