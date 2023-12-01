using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using System.Globalization;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("_-_-_- Menus -_-_-_")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject subMenuActive;

    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject subMain;
    [SerializeField] GameObject subControls;
    [SerializeField] GameObject[] controlsPages;
    [SerializeField] GameObject controlsPageActive;
    [SerializeField] GameObject subCredits;
    [SerializeField] GameObject subSettings;
    [SerializeField] GameObject[] settingsPages;
    [SerializeField] GameObject settingsPageActive;

    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuConfirmExit;
    [SerializeField] GameObject menuTESTING; //
    [SerializeField] GameObject menuInventory;

    [Header("_-_-_- HUD -_-_-_")]

    [SerializeField] GameObject hud;

    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text tempGoldCountText;
    [SerializeField] TMP_Text totalGoldCountText;

    [SerializeField] GameObject dialogDisplay;
    [SerializeField] GameObject promptDisplay;

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
    [SerializeField] AudioClip buttonPressed;


    public bool isPaused;

    public bool unlockedDoors;
    public bool unlockedHealthKits;
    public bool unlockedAmmoKits;
    
    public bool playerUnkillable = false;
    public bool infiniteAmmo = false;
    public bool beybladebeybladeLETITRIP = false;

    bool inDialog;
    string dialogCurrent;
    bool firstDialog = true;

    float timescaleOrig;
    int enemiesRemaining;

    List<spawner> spawnersList = new List<spawner>();

    int totalGold;
    int tempGold;

    GameObject menuPrevious;
    int controlsPage;

    bool userReady = false;

    void Awake()
    {
        instance = this;
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("Respawn");

        statePause();
        menuActive = menuMain;
        subMenuActive = subMain;
        subMenuActive.SetActive(true);
        menuActive.SetActive(true);
    }

    private void Start()
    {
        userReady = true;
    }

    void Update()
    {
        if(inDialog && Input.anyKeyDown)
        {
            ShowDialog(dialogCurrent);
        }
        else if(Input.GetButtonDown("Cancel") && !isPaused)
        {
            showPauseMenu();
        }
    }

    //

    public void startRun()
    {
        stateUnPause();
        AddTempGold(-tempGold);
        enemiesRemaining = 0;

        playerScript.SpawnPlayer();

        for(int i = 0; i < spawnersList.Count; i++)
        {
            spawnersList[i].resetSpawn();
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (menuActive != null)
        { menuActive.SetActive(false); }
        menuActive = null;
    }

    //Show Menus
    public void showMainMenu()
    {
        menuActive.SetActive(false);
        menuPrevious = menuActive;
        menuActive = menuMain;
        subMenuActive = subMain;
        subMenuActive.SetActive(true);
        menuActive.SetActive(true);
    }
    public void showMain()
    {
        controlsPageActive.SetActive(false);
        subMenuActive.SetActive(false);
        subMenuActive = subMain;
        subMenuActive.SetActive(true);
    }

    public void showPauseMenu()
    {
        statePause();
        menuActive = menuPause;
        menuActive.SetActive(true);
    }

    public void showControls()
    {
        subMenuActive.SetActive(false);
        subMenuActive = subControls;
        switchControlsPage(-controlsPage);
        subMenuActive.SetActive(true);
    }

    public void switchControlsPage(int change)
    {
        if(controlsPage + change < controlsPages.Length && controlsPage + change >= 0)
        {
            controlsPage += change;
        }

        controlsPageActive.SetActive(false);
        controlsPageActive = controlsPages[controlsPage];
        controlsPageActive.SetActive(true);
    }

    public void showSettings()
    {
        subMenuActive.SetActive(false);
        subMenuActive = subSettings;
        switchSettingsPage(0);
        subMenuActive.SetActive(true);
    }
    public void switchSettingsPage(int page)
    {
        if (page >= settingsPages.Length || page < 0)
        {
            return;
        }

        settingsPageActive.SetActive(false);
        settingsPageActive = settingsPages[page];
        settingsPageActive.SetActive(true);
    }

    public void showCredits()
    {
        subMenuActive.SetActive(false);
        subMenuActive = subCredits;
        subMenuActive.SetActive(true);
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

    public void ammoInfinite()
    {
        infiniteAmmo = !infiniteAmmo;
    }

    public void LETITRIP()
    {
        beybladebeybladeLETITRIP = !beybladebeybladeLETITRIP;
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
        statePause();
        //gameOver();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    /// <summary>
    /// Opens inventory menu.
    /// </summary>
    public void exitToInventory()
    {
        menuActive.SetActive(false);
        ShowPrompt(false);
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
	public bool updateEnemyCount(int amount)
    {
        enemiesRemaining += amount;
        enemyCountText.GetComponent<TMP_Text>().text = enemiesRemaining.ToString("0");
        if (enemiesRemaining <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
	/// Adds a spawner to the spawnersList.
	/// </summary>
	/// <param name="toAdd"></param>
	public void addSpawner(spawner toAdd)
    {
        spawnersList.Add(toAdd);
    }

    /// <summary>
    /// Adds to gold.
    /// </summary>
    /// <param name="amount"></param>
    public void addTotalGold(int amount)
    {
        totalGold += amount;
        totalGoldCountText.GetComponent<TMP_Text>().text = totalGold.ToString("0");
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

    public void ShowPrompt(bool on, string prompt = "")
    {
        promptDisplay.SetActive(on);
        promptDisplay.GetComponentInChildren<TMP_Text>().text = prompt;
        if(prompt.Length > 30)
        {
            promptDisplay.GetComponentInChildren<TMP_Text>().fontSize = 66 - prompt.Length;
        }
    }

    public void ShowDialog(string dialog)
    {
        int charMax = (int)(5170/ dialogDisplay.GetComponentInChildren<TMP_Text>().fontSize);

        int cutIndex = 0;

        int space;
        int period;
        int forceBreak;
        bool final = false;

        if (dialog.Length <= 0)
        {
            inDialog = false;
            firstDialog = true;
            dialogDisplay.SetActive(false);
            stateUnPause();
            return;
        }

        isPaused = true;
        Time.timeScale = 0f;

        dialogDisplay.SetActive(true);
        inDialog = true;

        dialogCurrent = dialog;

        if (dialog.Length < charMax)
        {
            final = true;
        }
        else
        {
            space = dialog.LastIndexOf(" ", charMax);
            period = dialog.LastIndexOf(".", charMax);
            forceBreak = dialog.LastIndexOf("\n", charMax);

            if(forceBreak != -1)
            {
                cutIndex = forceBreak;
            }
            else if (period != -1 && period < space && space - period > charMax / 5)
            {
                cutIndex = period;
            }
            else if (space != -1)
            {
                cutIndex = space;
            }
            else
            {
                cutIndex = charMax - 1;
            }
        }

        if (final)
        {
            dialogDisplay.GetComponentInChildren<TMP_Text>().text = dialog;
            dialogCurrent = "";
            return;
        }

        dialogDisplay.GetComponentInChildren<TMP_Text>().text = dialog.Remove(cutIndex + 1);

        if (firstDialog)
        {
            firstDialog = !firstDialog;
        }
        else
        {
            dialogCurrent = dialog.Remove(0, cutIndex + 1);
        }
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

    public void PlaySound(AudioClip clip, float vol = 0.5f)
    {
        if (userReady)
        {
            source.PlayOneShot(clip, vol);
        }
    }

    public void PlayButtonPress()
    {
        if (userReady)
        {
           if(buttonPressed != null) source.PlayOneShot(buttonPressed);
        }
    }

    public void ChangeTextSize()
    {
        Vector3 changeScale = new Vector3(settingsManager.sm.textSize, settingsManager.sm.textSize, settingsManager.sm.textSize);
        
        hud.transform.localScale = changeScale;

        promptDisplay.transform.localScale = changeScale;

        dialogDisplay.GetComponentInChildren<TMP_Text>().fontSize = 36 * settingsManager.sm.textSize;
    }

}
