using UnityEngine;
using System.Collections;

public class FoxAnimation : MonoBehaviour
{
    public static FoxAnimation instance;

 [Header("Idle Animation Sets")]
public Sprite[] idleFrames;           // regular
public Sprite[] goldenFrames1;        // golden tier 1
public Sprite[] goldenFrames2;        // golden tier 2
public Sprite[] vinesFrames;          // vines
public Sprite[] vinesWingsFrames;     // vines + wings

    private Sprite[] activeFrames;      // whichever is currently playing

    [Header("Scare Shake")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float shakeSpeed = 25f;

    [Header("Playback")]
    public float fps = 8f;

    private SpriteRenderer sr;
    private Vector3 originalPosition;
    private bool isShaking = false;
    private bool playingIdle = true;
    private float frameTimer;
    private int currentFrame;

    void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
        originalPosition = transform.localPosition;
        activeFrames = idleFrames; // start with regular fox
    }

    void Update()
    {
        if (playingIdle && activeFrames != null && activeFrames.Length > 0)
        {
            frameTimer += Time.deltaTime;
            if (frameTimer >= 1f / fps)
            {
                frameTimer = 0f;
                currentFrame = (currentFrame + 1) % activeFrames.Length;
                sr.sprite = activeFrames[currentFrame];
            }
        }
    }

    // Call these from UpgradeManager when upgrades are bought
    public void SetNormalFox()
    {
        SwitchFrames(idleFrames);
    }

    public void SetGoldenFox1()     
    { SwitchFrames(goldenFrames1); }
    public void SetGoldenFox2()     
    { SwitchFrames(goldenFrames2); }

    public void SetVinesFox()       
    { SwitchFrames(vinesFrames); }
    public void SetVinesWingsFox()  
    { SwitchFrames(vinesWingsFrames); }

    void SwitchFrames(Sprite[] newFrames)
    {
        activeFrames = newFrames;
        currentFrame = 0;
        frameTimer = 0f;
    }

    public void PlayScareShake()
    {
        if (!isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    public void StopIdle() { playingIdle = false; }
    public void ResumeIdle() { playingIdle = true; }

    IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        float timer = 0f;

        while (timer < shakeDuration)
        {
            float offsetX = Mathf.Sin(timer * shakeSpeed) * shakeMagnitude;
            float offsetY = Mathf.Sin(timer * shakeSpeed * 0.7f) * shakeMagnitude * 0.5f;
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        isShaking = false;
    }
}