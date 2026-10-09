using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;
    [Header("LevelText")]
    [SerializeField] private TMPro.TMP_Text levelText;
    public void Start()
    {
        LevelTextRefresh();
    }

    public void LevelTextRefresh()
    {
        levelText.text = "Level: " + SaveSystem.GetLevel().ToString();
    }
    public void ContinueGame()
    {
        //todo: Call elijah save and load system to load the game
        SceneManager.LoadScene("TrainingGrounds");// temporary until save and load system is implemented
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("TrainingGrounds");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
