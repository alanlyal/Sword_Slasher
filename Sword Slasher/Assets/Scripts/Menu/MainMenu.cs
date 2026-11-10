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
    [Header("LevelText")]
    [SerializeField] private TMPro.TMP_Text levelText;

    // Audio
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;
    public AudioMixer audioMixer;

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
