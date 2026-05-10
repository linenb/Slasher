using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        audioSource.volume = savedVolume;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}