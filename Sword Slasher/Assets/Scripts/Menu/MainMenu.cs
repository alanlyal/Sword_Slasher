using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject updatePanel;
    [Header("LevelText")]
    [SerializeField] private TMPro.TMP_Text levelText;
    [SerializeField] private Toggle tutorialToggle;

    // Audio
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;
    public AudioMixer audioMixer;
    
    // Update Info
    [SerializeField] private TMPro.TMP_Text updateTitle;
    [SerializeField] private TMPro.TMP_Text updateInfo;

    float sfxValue;
    float bgmValue;

    public void Start()
    {
        SaveSystem.LoadPlayerData();
        LevelTextRefresh();

        audioMixer.GetFloat("SFXvolume", out sfxValue);
        sfxSlider.value = sfxValue;
        audioMixer.GetFloat("BGMvolume", out bgmValue);
        bgmSlider.value = bgmValue;
        MusicManager.Play("MenuMusic");
        tutorialToggle.isOn = PlayerPrefs.GetInt("ShowTutorial") == 1;
        
        //hardcoded update for now as we are unlikely to update a live version of this game
        SetUpdateText("Release 2: Iteration 2",
            $"- Added toggle for the tutorial \n" + 
            $"- Added Sword Customization \n" + 
            $"- Added Leaderboard \n" +
            $"- Added update log \n" +
            $"- Added quests");
    }

    public void LevelTextRefresh()
    {
        levelText.text = "Level: " + SaveSystem.GetLevel().ToString();
    }
    public void ContinueGame()
    {
        if (!SaveSystem.hasPlayerDataSaved())
        {
            Debug.Log("no save file found");
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
    public void OpenUpdate()
    {
        updatePanel.SetActive(true);
    }
    public void CloseUpdate()
    {
        updatePanel.SetActive(false);
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

    public void ToggleTutorial(bool isOn)
    {
        PlayerPrefs.SetInt("ShowTutorial", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetUpdateText(string title, string info)
    {
        updateTitle.text = title;
        updateInfo.text = info;
    }
}
