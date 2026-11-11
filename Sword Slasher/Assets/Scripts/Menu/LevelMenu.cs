using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject statPanel;
    [SerializeField] private GameObject customizationPanel;
    [SerializeField] private GameObject tutorialPanel;

    [Header("Audio")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;
    public AudioMixer audioMixer;

    float sfxValue;
    float bgmValue;

    [Header("Stats")]
    [SerializeField] public int level = 1;
    [SerializeField] public int levelXp = 100;
    [SerializeField] public int xp;
    [SerializeField] public int passiveXP;
    [SerializeField] public int skillPoints = 0;
    [SerializeField] public int xpPerClick = 10;
    [SerializeField] public int passiveClick = 0;
    [SerializeField] public int totalClicks;
    [SerializeField] public int totalPassiveClicks;
    [SerializeField] public int upgradesOwned;

    //allows the level menu to be called by other classes to use the public variables
    [Header("XP bar")]
    [SerializeField] private Slider xpBar;
    [Header("Level Text")]
    [SerializeField] private TMP_Text lvTxt;
    private int previousLevelUpXp = 0;

    private float timer = 0f;
    private float interval = 1f;

    private static LevelMenu instance;
    public static LevelMenu Instance
    {
        get
        {
            if (instance != null) { return instance; }
            instance = FindAnyObjectByType(typeof(LevelMenu)) as LevelMenu;
            if (instance == null) { Debug.Log("LevelMenu not found"); }

            return instance;
        }
    }

    private void Start()
    {
        if (SaveSystem.isContinuing)
        {
            tutorialPanel.SetActive(false);
            LoadGame(SaveSystem.playerData);
        }
        else
        {
            //ensuring the values are what they should be at start
            level = 1;
            levelXp = 100;
            xp = 0;
            xpPerClick = 10;
            totalClicks = 0;
            previousLevelUpXp = 0; // resets the xpBar to 0 when the game starts
            ResetXpBar(); // resets the xpBar to mirror the current xp and level
            UpdateLvTxt();// reset the lv text on startup
            tutorialPanel.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        ResetXpBar();
        UpdateLvTxt();
        audioMixer.GetFloat("SFXvolume", out sfxValue);
        audioMixer.GetFloat("BGMvolume", out bgmValue);
        sfxSlider.value = sfxValue;
        bgmSlider.value = bgmValue;
        MusicManager.Play("GameplayMusic");
    }

    //For Upgrades that generate passive clicks
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if (passiveClick >0)
            {
                totalPassiveClicks += passiveClick;
                passiveXP = totalPassiveClicks * 10;
                AddXp(passiveXP);
                totalPassiveClicks = 0;
            }
            timer -= interval;
        }
    }

    public void OpenUpgradePanel()
    {
        gamePanel.SetActive(false);
        upgradePanel.SetActive(true);
        optionPanel.SetActive(false);
        statPanel.SetActive(false);
        customizationPanel.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
    public void OpenOptionPanel()
    {
        gamePanel.SetActive(false);
        optionPanel.SetActive(true);
        upgradePanel.SetActive(false);
        statPanel.SetActive(false);
        customizationPanel.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }
    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
    public void ResumeGame()
    {
        gamePanel.SetActive(true);
        upgradePanel.SetActive(false);
        optionPanel.SetActive(false);
        statPanel.SetActive(false);
        customizationPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Quit Game");
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void OpenStatPanel()
    {
        gamePanel.SetActive(false);
        statPanel.SetActive(true);
        upgradePanel.SetActive(false);
        optionPanel.SetActive(false);
        customizationPanel.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }
    public void CloseStatPanel()
    {
        statPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    //Customization panel
    public void OpenCustomizationPanel()
    {
        gamePanel.SetActive(false);
        customizationPanel.SetActive(true);
        optionPanel.SetActive(false);
        statPanel.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    public void CloseCustomizationPanel()
    {
        customizationPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void CloseTutorialPanel()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }



    public void SaveGame()
    {
        var data = SaveSystem.playerData;
        data.level = level;
        data.xp = xp;
        data.levelXp = levelXp;
        data.clickingXp = xpPerClick;
        data.skillPoints = skillPoints;
        data.previousLevelUpXp = previousLevelUpXp;
        data.passiveClicks = passiveClick;
        data.totalClicks = totalClicks;
        data.upgradesBought = upgradesOwned;
        data.upgrades.Clear();
        foreach(var button in FindObjectsByType<UpgradeButton>(FindObjectsInactive.Include))
        {
            data.upgrades.Add(button.upgradeId + ":" + button.upgradesOwned);
        }
        if (AchievementManager.Instance != null)
        {
            data.achievements = AchievementManager.Instance.GetUnlocked();
        }
        var customization = FindAnyObjectByType<Customization>(FindObjectsInactive.Include);
        if (customization != null)
        {
            data.custimization = customization.OptionSelected;
        }
        SaveSystem.saveData();
        Debug.Log("Game saved");
    }

    public void AddClick(int i)
    {
        totalClicks += i;
        GameEvents.Click(totalClicks);
        AddXp(xpPerClick);
        Debug.Log($"You have clicked {totalClicks} times"); //turn this into a UI element later
    }

    public void AddXp(int i)
    {
        xp += i;
        LevelUp();
        ResetXpBar();//resets the xp bar to mirror the current xp and level
        Debug.Log($"You have {xp} XP"); //turn into UI element later
        ResetXpBar();
    }

    public void LevelUp()
    {
        if (xp >= levelXp && level <20 ) // Will have to change depending on how many upgrades
        {
            level++;
            skillPoints++;
            GameEvents.LevelUp(level);
            previousLevelUpXp = levelXp; //resets the bar once the player levels up so it doesn't reset to 0
            UpdateLvTxt();// to update the level text
            Debug.Log($"Skill Points: {skillPoints}"); //Quick Skillpoint Log
            if (level < 20)
            {
                levelXp += levelXp + 100; //makes each level take double the xp of the previous one
                Debug.Log($"You are level {level}, you need {levelXp} XP to get to the next level"); //turn into UI element later
                ResetXpBar();

            }
            else
            {
                Debug.Log("Congrats, you have reached the max level of 20, you still earn xp but no more levels"); //turn into UI element later?
            }
        }
    }
    public void ResetXpBar() // function to reset the xp bar to the correct value based on the current xp and levelXp
    {
        if (xpBar == null) return;

        if (level >= 20) // once the player hits the max level. the bar just gets maxes out
        {
            xpBar.minValue = 0;
            xpBar.maxValue = 1;
            xpBar.value = 1;
            return;
        }
        xpBar.maxValue = levelXp;
        xpBar.minValue = previousLevelUpXp;
        xpBar.value = xp;

    }
    private void UpdateLvTxt() // function to update the levelUpText
    {
        if (lvTxt == null)
        {
            return;
        }
        lvTxt.text = "Level: " + level;
    }
    private void LoadGame(PlayerData data)
    {
        level = data.level;
        xp = data.xp;
        previousLevelUpXp = data.previousLevelUpXp;
        xpPerClick = data.clickingXp;
        skillPoints = data.skillPoints;
        passiveClick = data.passiveClicks;
        totalClicks = data.totalClicks;
        levelXp = data.levelXp;
        upgradesOwned = data.upgradesBought;
        foreach (var button in FindObjectsByType<UpgradeButton>(FindObjectsInactive.Include))
        {
           foreach (string savedUpgrade in data.upgrades)
            {
                string[] nameAndAmount = savedUpgrade.Split(':');
                if (nameAndAmount[0] == button.upgradeId)
                {
                    button.RestoreUpgrades(int.Parse(nameAndAmount[1]));
                }
            }
        }
        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.RestoreUnlocked(data.achievements);
        }
        var customization = FindAnyObjectByType<Customization>(FindObjectsInactive.Include);
        if(customization != null)
        {
            customization.LoadSelectedOption();
        }
    }
    

    public void SfxVolume(float volume)
    {
        if (volume == -25)
        {
            audioMixer.SetFloat("SFXvolume", -80);
        }
        else
        {
            audioMixer.SetFloat("SFXvolume", volume);
        }
    }

    public void BgmVolume(float volume)
    {
        if (volume == -35)
        {
            audioMixer.SetFloat("BGMvolume", -80);
        }
        else
        {
            audioMixer.SetFloat("BGMvolume", volume);
        }
    }
}
