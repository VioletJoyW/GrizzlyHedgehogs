using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using System.Globalization;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public int curScene = 0;


    public enum MenuType
    {
        None = 0x0,
        NORMAL_MENU,
        INTRO_MENU,
        TEST
    }

    [Header("_-_-_- Settings -_-_-_")]
    [SerializeField] MenuType menu = MenuType.None;
    [SerializeField] bool activatePlayerAtStart = true;
    [Header("_-_-_- Cinema Settings -_-_-_")]
    [SerializeField] GameObject cinemaCamera;

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
    [SerializeField] GameObject menuTESTING;
    [SerializeField] GameObject menuInventory;

    [Header("_-_-_- HUD -_-_-_")]

    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text objectiveText;
    [SerializeField] TMP_Text powerText;
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
    [SerializeField] TMP_Text gunNameText;
    [SerializeField] TMP_Text playerAmmoText;
    [SerializeField] Image playerAmmoBackground;
	[Header("_-_-_- HUD Objects -_-_-_")]
    [SerializeField] GameObject hud;
    [SerializeField] GameObject hudMap;
	[SerializeField] GameObject gunAmmoHUDObject;
    [SerializeField] GameObject staminaHUDObject;
    [SerializeField] GameObject healthHUDObject;
    [SerializeField] GameObject keysHUDObject;
    [SerializeField] GameObject enemyCountHUDObject;
    [SerializeField] GameObject objectiveHUDObject;
    [SerializeField] GameObject powerHUDObject;

    [Header("_-_-_- Player Info -_-_-_")]
    public GameObject playerSpawnPos;
    public GameObject player;
    public playerController playerScript;

    [Header("_-_-_- Audio Files -_-_-_")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip buttonPressed;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] musicClips;

    public bool isPaused;

    public bool unlockedDoors;
    public bool unlockedHealthKits;
    public bool unlockedAmmoKits;

    public bool playerUnkillable = false;
    public bool infiniteAmmo = false;
    public bool beybladebeybladeLETITRIP = false;
    public bool bHead = false;

    bool inDialog;
    string dialogCurrent;
    bool firstDialog = true;

    float timescaleOrig;
    int enemiesRemaining;

    List<spawner> spawnersList = new List<spawner>();

    GameObject menuPrevious;
    int totalGold;
    int tempGold;
    int controlsPage;
    int musicCurr = 1;
    int dialogTimer = -1;

    bool wasMainMenuTriggered = false;

    bool cCamWasActive;
	void Awake()
    {
        if(cinemaCamera != null)
        {
            cCamWasActive = cinemaCamera.activeSelf;
            cinemaCamera.SetActive(false);
        }
        instance = this;
        Utillities.CreateGlobalTimer(5.0f, ref dialogTimer);
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("Respawn");
        SceneLoaderObj.RunScripts();
        player.SetActive(activatePlayerAtStart);
        hud.SetActive(false);
        hudMap.SetActive(false);
		if (cinemaCamera != null)
			cinemaCamera.SetActive(cCamWasActive);
        switch (menu) // Menu Controls
        {
            case MenuType.NORMAL_MENU:
                ShowMainMenu(); break;

            case MenuType.INTRO_MENU:
                ShowIntroMenu();
                break;

            case MenuType.TEST:
                
                break;


            default: break;
        }

        //musicCurr = SceneManager.GetActiveScene().buildIndex + 1; //This won't act right in scenes that don't have a build index
    }

    private void Start()
    {
        if (menu == MenuType.None)
        {
            stateUnPause();
            menuMain.SetActive(false);
        }
    }

    public void ShowIntroMenu()
    {
        menuMain.SetActive(false);
    }

    public void ShowMainMenu()
    {
        wasMainMenuTriggered = true;

		statePause();
		menuActive = menuMain;
		subMenuActive = subMain;
		subMenuActive.SetActive(true);
		menuActive.SetActive(true);
	}


    bool confirm = false;

	public bool WasMainMenuTriggered { get => wasMainMenuTriggered; }
	public bool ActivatePlayerAtStart { get => activatePlayerAtStart; set => activatePlayerAtStart = value; }

	void Update()
    {
        confirm = Input.GetKeyUp(KeyCode.E);

        if (!wasMainMenuTriggered)
        {
            switch(menu)
            {
                case MenuType.INTRO_MENU:
                    {

                    }
                    break;
            }
        }

		if (inDialog && confirm)
        {
            ShowDialog(dialogCurrent);
            Utillities.ResetGlobalTimer(dialogTimer);
            confirm = false;
        }
        else if(Input.GetButtonDown("Cancel") && !isPaused)
        {
            showPauseMenu();
        }
        Utillities.UpdateGlobalTimer(dialogTimer);
    }

    //

    public void startRun()
    {
        stateUnPause();
        AddTempGold(-tempGold);
        enemiesRemaining = 0;

		hud.SetActive(true);
		hudMap.SetActive(true);
        playerScript.SpawnPlayer();
		for (int i = 0; i < spawnersList.Count; i++)
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

        //if (musicSource.clip == musicClips[0])
        //{
            ChangeMusic(musicCurr);
        //}
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
        ChangeMusic(0);
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

    public void bigHead()
    {
        bHead = !bHead;
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
		return (enemiesRemaining <= 0);
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

    public void UpdatePlayerUI(int healthCurrent, int healthMax, float staminaCurrent, float staminaMax, ScriptableGunStats gun, int ammoTotal)
    {
        playerHealthBar.fillAmount = (float)healthCurrent / healthMax;
        playerHealthText.text = healthCurrent.ToString("0") + " / " + healthMax.ToString("0");
        playerStaminaBar.fillAmount = staminaCurrent / staminaMax;
        playerStaminaText.text = staminaCurrent.ToString("0") + " / " + staminaMax.ToString("0");
        playerAmmoBar.fillAmount = (float)gun.ammoCurrent / gun.ammoMax;
        playerAmmoText.text = gun.ammoCurrent.ToString("0") + " / " + ammoTotal.ToString("0");
        gunNameText.text = gun.name;
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

    public void ChangeMusic(int clip)
    {
        if(clip >= musicClips.Length || clip < 0)
        {
            return;
        }
        else if(clip != 0)
        {
            musicCurr = clip;
        }

        musicSource.clip = musicClips[clip];
        musicSource.Play();
    }

    public void ChangeMusicVol()
    {
        musicSource.volume = settingsManager.sm.settingsCurr.musicVol * .2f;
    }

    public void PlaySound(AudioClip clip, float vol = 0.5f)
    {
        source.PlayOneShot(clip, vol);
    }

    public void PlayButtonPress()
    {
        if (buttonPressed != null)
        {
            source.PlayOneShot(buttonPressed);
        }
    }

    public void ChangeTextSize()
    {
        Vector3 changeScale = new Vector3(settingsManager.sm.settingsCurr.textSize, settingsManager.sm.settingsCurr.textSize, settingsManager.sm.settingsCurr.textSize);

        hud.transform.localScale = changeScale;

        promptDisplay.transform.localScale = changeScale;

        dialogDisplay.GetComponentInChildren<TMP_Text>().fontSize = 36 * settingsManager.sm.settingsCurr.textSize;
    }

    public void UpdateGameObjective(string objective)
    {
        objectiveText.text = objective;
    }

    public void UpdatePowerText()
    {
        if (playerScript.GetPowerBuffer().IsActive && playerScript.GetPowerBuffer().Count > 0)
        {
            powerText.text = playerScript.GetPowerBuffer().GetCurrentPower.name;
        }
        else
        {
            powerText.text = "None";
        }
    }

    public void NextLevel()
    {
        curScene++;
        SceneManager.LoadScene(curScene);
    }
}
