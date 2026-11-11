using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private static AudioSource audioSource;
    private static MusicLibrary musicLibrary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            musicLibrary = GetComponent<MusicLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = musicLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public static void Stop()
    {
        audioSource.Stop();
    }
}
