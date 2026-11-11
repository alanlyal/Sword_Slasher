using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    private static SFXManager instance;

    [SerializeField] private static AudioSource audioSource;
    private static SFXLibrary soundEffectLibrary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SFXLibrary>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HookButtons();
    }

    public static void HookButtons()
    {
        var buttons = FindObjectsByType<Button>(FindObjectsInactive.Include);
        foreach (var b in buttons)
        {
            b.onClick.RemoveListener(PlayClick);
            b.onClick.AddListener(PlayClick);
        }
    }

    private static void PlayClick() => Play("ButtonClick");

    //to play a sound call "SFXManager.Play("soundname"); on the given object
    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
