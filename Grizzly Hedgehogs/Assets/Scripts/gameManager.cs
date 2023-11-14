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


    public bool isPaused;

    public bool unlockedDoors;
    public bool unlockedHealthKits;
    public bool unlockedAmmoKits;
    
    bool playerUnkillable = false;

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
        AddTempGold(-tempGold);

        playerScript.SpawnPlayer();

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

    //Functions for testing
    public void goldSlider(Slider amount)
    {
        tempGold = (int)amount.value;
        tempGoldCountText.text = tempGold.ToString("0");

        totalGold = (int)amount.value;
        totalGoldCountText.text = totalGold.ToString("0");
    }
    public void healthSlider(Slider amount)
    {
        playerScript.AddHealth(-playerScript.GetHealth() + (int)amount.value);
    }

    public void unkillable()
    {
        playerUnkillable = !playerUnkillable;
    }
    
    /// <summary>
    /// Displays win screen.
    /// </summary>
    public void youWin()
    {
        statePause();
        //gameWin();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    /// <summary>
    /// Displays lose screen.
    /// </summary>
    public void youLose()
    {
        if(playerUnkillable)
        {
            playerScript.AddHealth(100);
            return;
        }

        statePause();
        //gameOver();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    /// <summary>
    /// Closes inventory menu.
    /// </summary>
    public void exitToInventory()
    {
        menuActive.SetActive(false);
        ShowInteractPrompt(false);
        ShowLockedPrompt(false);
        menuActive = menuInventory;
        addTotalGold(tempGold);
        menuActive.SetActive(true);
    }

	/// <summary>
	/// Goes back one menu.
	/// </summary>
	public void goBack()
    {
        menuActive.SetActive(false);
        menuActive = menuPrevious;
        menuActive.SetActive(true);
    }


	/// <summary>
	/// Adds to enemy count.
	/// </summary>
	/// <param name="amount"></param>
	public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemyCountText.text = enemiesRemaining.ToString("0");
        if (enemiesRemaining <= 0)
        {
            youWin();
        }
    }

	/// <summary>
	/// Adds to gold.
	/// </summary>
	/// <param name="amount"></param>
	public void addTotalGold(int amount)
    {
        totalGold += amount;
        totalGoldCountText.text = totalGold.ToString("0");
    }

	/// <summary>
	/// Gets total gold.
	/// </summary>
	/// <returns></returns>
	public int GetTotalGold()
    {
        return totalGold;
    }

	/// <summary>
	/// Adds to temp gold. (should this be depracted?)
	/// </summary>
	/// <param name="amount"></param>
	public void AddTempGold(int amount)
    {
        tempGold += amount;
        tempGoldCountText.text = tempGold.ToString("0");
    }

    //

    public void UpdatePlayerUI(int healthCurrent, int healthMax, float staminaCurrent, float staminaMax, int ammoCurrent, int ammoMax)
    {
        playerHealthBar.fillAmount = (float)healthCurrent / healthMax;
        playerHealthText.text = healthCurrent.ToString("0") + " / " + healthMax.ToString("0");
        playerStaminaBar.fillAmount = staminaCurrent / staminaMax;
        playerStaminaText.text = staminaCurrent.ToString("0") + " / " + staminaMax.ToString("0");
        playerAmmoBar.fillAmount = (float)ammoCurrent / ammoMax;
        playerAmmoText.text = ammoCurrent.ToString("0") + " / " + ammoMax.ToString("0");
    }

    public void ShowInteractPrompt(bool on)
    {
        interactPrompt.SetActive(on);
    }

    public void ShowLockedPrompt(bool on)
    {
        lockedPrompt.SetActive(on);
    }

    public IEnumerator PlayerFlashDamage()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        playerDamageScreen.SetActive(false);
    }

    public IEnumerator AmmoFlashRed()
    {
        Color orig = playerAmmoBackground.color;
        playerAmmoBackground.color = Color.red;
        yield return new WaitForSeconds(.1f);
        playerAmmoBackground.color = orig;
    }

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

}
