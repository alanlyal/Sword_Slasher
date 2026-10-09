using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject achivementPanel;
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
}
