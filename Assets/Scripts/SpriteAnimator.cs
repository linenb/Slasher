using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Plays a sequence of sprites (extracted GIF frames) on a UI Image component.
/// Attach this to the same GameObject as your Image.
/// </summary>
[RequireComponent(typeof(Image))]
public class SpriteAnimator : MonoBehaviour
{
    [Header("Frames")]
    [Tooltip("Drag your extracted GIF frame sprites here in order.")]
    public Sprite[] frames;

    [Header("Playback")]
    [Tooltip("Frames per second.")]
    public float fps = 12f;

    [Tooltip("Should the animation loop?")]
    public bool loop = true;

    [Tooltip("Play automatically when the object is enabled.")]
    public bool playOnEnable = true;

    private Image image;
    private Coroutine animationCoroutine;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void OnEnable()
    {
        if (playOnEnable)
            Play();
    }

    void OnDisable()
    {
        Stop();
    }

    public void Play()
    {
        if (frames == null || frames.Length == 0)
        {
            Debug.LogWarning($"[SpriteAnimator] No frames assigned on {gameObject.name}");
            return;
        }

        Stop(); // Stop any existing coroutine before starting a new one
        animationCoroutine = StartCoroutine(Animate());
    }

    public void Stop()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    private IEnumerator Animate()
    {
        float delay = 1f / fps;
        int currentFrame = 0;

        while (true)
        {
            image.sprite = frames[currentFrame];
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (!loop) yield break; // Stop after one cycle if not looping
                currentFrame = 0;
            }

            // Use unscaled time so animation still plays when Time.timeScale = 0
            yield return new WaitForSecondsRealtime(delay);
        }
    }
}