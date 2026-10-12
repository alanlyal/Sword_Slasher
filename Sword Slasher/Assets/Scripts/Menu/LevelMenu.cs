using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.Properties;

public class LevelMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject statPanel;

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
        //ensuring the values are what they should be at start
        level = 1;
        levelXp = 100;
        xp = 0;
        xpPerClick = 10;
        totalClicks = 0;
        previousLevelUpXp = 0; // resets the xpBar to 0 when the game starts
        ResetXpBar(); // resets the xpBar to mirror the current xp and level
        UpdateLvTxt();// reset the lv text on startup
    }

    //For Upgrades that generate passive clicks
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            totalPassiveClicks += passiveClick;
            passiveXP = totalPassiveClicks * 10;
            AddXp(passiveXP);
            totalPassiveClicks = 0;
            timer -= interval;
        }
    }

    public void OpenUpgradePanel()
    {
        gamePanel.SetActive(false);
        upgradePanel.SetActive(true);
        optionPanel.SetActive(false);
        statPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
    }
    public void OpenOptionPanel()
    {
        gamePanel.SetActive(false);
        optionPanel.SetActive(true);
        upgradePanel.SetActive(false);
        statPanel.SetActive(false);
        Time.timeScale = 0f;
    }
    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
    }
    public void ResumeGame()
    {
        gamePanel.SetActive(true);
        upgradePanel.SetActive(false);
        optionPanel.SetActive(false);
        statPanel.SetActive(false);
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
        Time.timeScale = 0f;
    }
    public void CloseStatPanel()
    {
        statPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
    }
    public void SaveGame()
    {
        //TODO
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
        if (xp >= levelXp && level < 101) // Will have to change depending on how many upgrades
        {
            level++;
            skillPoints++;
            GameEvents.LevelUp(level);
            previousLevelUpXp = levelXp; //resets the bar once the player levels up so it doesn't reset to 0
            UpdateLvTxt();// to update the level text
            Debug.Log($"Skill Points: {skillPoints}"); //Quick Skillpoint Log
            if (level < 100)
            {
                levelXp += levelXp + 100; //makes each level take double the xp of the previous one
                Debug.Log($"You are level {level}, you need {levelXp} XP to get to the next level"); //turn into UI element later
                ResetXpBar();

            }
            else
            {
                Debug.Log("Congrats, you have reached the max level of 100, you still earn xp but no more levels"); //turn into UI element later?
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
}
