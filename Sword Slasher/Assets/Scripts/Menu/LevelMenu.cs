using System;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject achivementPanel;

    [Header("Stats")]
    [SerializeField] private int level = 1;
    [SerializeField] private int levelXp = 100;
    [SerializeField] private int xp;
    [SerializeField] private int xpPerClick = 10;
    [SerializeField] private int totalClicks;

    //allows the level menu to be called by other classes to use the public variables
    private static LevelMenu instance;
    public static LevelMenu Instance
    {
        get
        {
            if (instance != null) { return instance; }
            instance = FindObjectOfType(typeof(LevelMenu)) as LevelMenu;
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
    }

    public void OpenUpgradePanel()
    {
        gamePanel.SetActive(false);
        upgradePanel.SetActive(true);
        optionPanel.SetActive(false);
        achivementPanel.SetActive(false);
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
        achivementPanel.SetActive(false);
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
        achivementPanel.SetActive(false);
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
    public void OpenAchivementPanel()
    {
        gamePanel.SetActive(false);
        achivementPanel.SetActive(true);
        upgradePanel.SetActive(false);
        optionPanel.SetActive(false);
        Time.timeScale = 0f;
    }
    public void CloseAchivementPanel()
    {
        achivementPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
    }
    public void SaveGame()
    { 
        //TODO
    }

    public void AddClick(int i)
    {
        totalClicks+= i;
        AddXp(xpPerClick);
        Debug.Log($"You have clicked {totalClicks} times"); //turn this into a UI element later
    }

    public void AddXp(int i)
    {
        xp += i;
        LevelUp();
        Debug.Log($"You have {xp} XP"); //turn into UI element later
    }

    public void LevelUp()
    {
        if (xp >= levelXp && level < 20)
        {
            level++;
            if (level < 20)
            {
                levelXp += levelXp + 100; //makes each level take double the xp of the previous one
                Debug.Log($"You are level {level}, you need {levelXp} XP to get to the next level"); //turn into UI element later
            }
            else
            {
                Debug.Log("Congrats, you have reached the max level of 20, you still earn xp but no more levels"); //turn into UI element later?
            }
        }
    }
}
