using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("UI Sounds")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip purchaseSound;
    public AudioClip errorSound;
    public AudioClip glassTapSound;
    public AudioClip ropePullSound;

    [Header("Creature Sounds")]
    public AudioClip batScreechSound;
    public AudioClip foxGruntSound;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;

    [Header("Minigame")]
    public float minPitch = 1f;
    public float pitchIncrease = 0.03f;
    public float maxPitch = 2f;

    private float currentPitch = 1f;
    private int foxClickCounter = 0;
    private bool foxGruntPlaying = false;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        LoadVolume();
    }

    void LoadVolume()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        audioSource.volume = masterVolume;
    }

    public void SetVolume(float volume)
    {
        masterVolume = volume;

        audioSource.volume = masterVolume;

        MusicManager music = FindFirstObjectByType<MusicManager>();

        if (music != null)
        {
            music.SetVolume(volume);
        }

        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }

    public void PlayHover()
    {
        PlaySound(hoverSound);
    }

    public void PlayClick()
    {
        PlaySound(clickSound);
    }

    public void PlayRopePull()
    {
        PlaySound(ropePullSound);
    }

    public void PlayPurchase()
    {
        PlaySound(purchaseSound);
    }

    public void PlayError()
    {
        PlaySound(errorSound);
    }

    public void PlayGlassTap()
    {
        PlaySound(glassTapSound);
    }

    public void PlayBatScreech()
    {
        PlaySound(batScreechSound);
    }

    public void PlaySpiderTap()
    {
        if (glassTapSound == null) return;

        audioSource.pitch = currentPitch;

        audioSource.PlayOneShot(glassTapSound, masterVolume);

        currentPitch += pitchIncrease;

        if (currentPitch > maxPitch)
        {
            currentPitch = maxPitch;
        }
    }

    public void ResetSpiderPitch()
    {
        currentPitch = minPitch;
        audioSource.pitch = 1f;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.pitch = 1f;

        audioSource.PlayOneShot(clip, masterVolume);
    }

    public void TryPlayFoxGrunt()
    {
        foxClickCounter++;

        // Random chance roughly every 10 clicks
        if (foxClickCounter >= Random.Range(8, 13))
        {
            foxClickCounter = 0;

            if (!foxGruntPlaying && foxGruntSound != null)
            {
                StartCoroutine(PlayFoxGruntRoutine());
            }
        }
    }

    IEnumerator PlayFoxGruntRoutine()
    {
        foxGruntPlaying = true;

        audioSource.pitch = Random.Range(0.95f, 1.05f);

        audioSource.PlayOneShot(foxGruntSound, masterVolume);

        yield return new WaitForSeconds(foxGruntSound.length);

        foxGruntPlaying = false;

        audioSource.pitch = 1f;
    }
}