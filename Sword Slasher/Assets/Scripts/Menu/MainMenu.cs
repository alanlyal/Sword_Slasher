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
        SaveSystem.LoadPlayerData();
        LevelTextRefresh();
    }

    public void LevelTextRefresh()
    {
        levelText.text = "Level: " + SaveSystem.GetLevel().ToString();
    }
    public void ContinueGame()
    {
        if (!SaveSystem.hasPlayerDataSaved())
        {
            Debug.Log("no save file found :(");
            return;
        }
        SaveSystem.isContinuing = true;
        SceneManager.LoadScene("TrainingGrounds");
    }

    public void PlayGame()
    {
        SaveSystem.isContinuing = false;
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
