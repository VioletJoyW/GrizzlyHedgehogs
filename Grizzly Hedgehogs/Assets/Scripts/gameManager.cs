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
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuControls;
    [SerializeField] GameObject menuCredits;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuConfirmExit;
    [SerializeField] GameObject menuTESTING; //
    [SerializeField] GameObject menuInventory;

    [Header("_-_-_- HUD -_-_-_")]
    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text tempGoldCountText;
    [SerializeField] TMP_Text totalGoldCountText;

    [SerializeField] GameObject notEnoughGoldMessage;
    [SerializeField] GameObject interactPrompt;
    [SerializeField] GameObject lockedPrompt;

    [SerializeField] Image playerHealthBar;
    [SerializeField] TMP_Text playerHealthText;
    [SerializeField] Image playerStaminaBar;
    [SerializeField] TMP_Text playerStaminaText;
    [SerializeField] Image playerAmmoBar;
    [SerializeField] TMP_Text playerAmmoText;
    [SerializeField] Image playerAmmoBackground;

    [Header("_-_-_- Player Info -_-_-_")]
    public GameObject playerSpawnPos;
    public GameObject player;
    public playerController playerScript;

    [Header("_-_-_- Audio Files -_-_-_")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip heal, coins, pShoot, eShoot, win, Death, Jump;

    public bool isPaused;

    public bool unlockedDoors;
    public bool unlockedHealthKits;
    public bool unlockedAmmoKits;

    float timescaleOrig;
    int enemiesRemaining;

    int totalGold;
    int tempGold;

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

    //

    public void startRun()
    {
        stateUnPause();
        addTempGold(-tempGold);

        playerScript.spawnPlayer();

        //Respawn Level stuff
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

    //Show Menus
    public void showMainMenu()
    {
        menuActive.SetActive(false);
        menuPrevious = menuActive;
        menuActive = menuMain;
        menuActive.SetActive(true);
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
    public void showConfirmExitMenu()
    {
        statePause();
        menuActive = menuConfirmExit;
        menuActive.SetActive(true);
    }

    public void showTESTINGMenu()
    {
        menuActive.SetActive(false);
        menuPrevious = menuActive;
        menuActive = menuTESTING;
        menuActive.SetActive(true);
    }

    //

    public void youWin()
    {
        statePause();
        gameWin();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }
    public void youLose()
    {
        statePause();
        gameOver();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void exitToInventory()
    {
        menuActive.SetActive(false);
        menuActive = menuInventory;
        addTempGold(tempGold);
        menuActive.SetActive(true);
    }

    public void goBack()
    {
        menuActive.SetActive(false);
        menuActive = menuPrevious;
        menuActive.SetActive(true);
    }

    //

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemyCountText.text = enemiesRemaining.ToString("0");
        if (enemiesRemaining <= 0)
        {
            youWin();
        }
    }

    public void addTotalGold(int amount)
    {
        totalGold += amount;
        totalGoldCountText.text = totalGold.ToString("0");
    }

    public int getTotalGold()
    {
        return totalGold;
    }

    public void addTempGold(int amount)
    {
        tempGold += amount;
        tempGoldCountText.text = tempGold.ToString("0");
    }

    //

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

    public void showLockedPrompt(bool on)
    {
        lockedPrompt.SetActive(on);
    }

    public IEnumerator showGoldMessage()
    {
        notEnoughGoldMessage.SetActive(true);
        yield return new WaitForSeconds(1);
        notEnoughGoldMessage.SetActive(false);
    }

    public IEnumerator playerFlashDamage()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        playerDamageScreen.SetActive(false);
    }

    public IEnumerator ammoFlashRed()
    {
        Color orig = playerAmmoBackground.color;
        playerAmmoBackground.color = Color.red;
        yield return new WaitForSeconds(.1f);
        playerAmmoBackground.color = orig;
    }

    #region audio

    public void healPlayer()
    {
        source.clip = heal;
        source.Play();
    }

    public void collectCoins()
    {
        source.clip = coins;
        source.Play();
    }

    public void gunPlayerShot()
    {
        source.clip = pShoot;
        source.Play();
    }
    public void gunEnemyShot()
    {
        source.clip = eShoot;
        source.Play();
    }

    public void playerJump()
    { 
        source.clip = Jump;
        source.Play();
    }

    public void gameWin()
    {
        source.clip = win;
        source.Play();
    }

    public void gameOver()
    {
        source.clip = Death;
        source.Play();
    }

    #endregion
}
