using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenAnimator : MonoBehaviour
{
    [Header("Elements to animate")]
    public CanvasGroup logo;
    public CanvasGroup newGameButton;
    public CanvasGroup continueButton;
    public CanvasGroup exitButton;

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // start everything invisible
        SetAlpha(logo, 0f);
        SetAlpha(newGameButton, 0f);
        SetAlpha(continueButton, 0f);
        SetAlpha(exitButton, 0f);

        // fade in logo first
        yield return StartCoroutine(FadeIn(logo, 1f));
        yield return new WaitForSeconds(0.3f);

        //buttons one by one
        yield return StartCoroutine(FadeIn(newGameButton, 0.5f));
        yield return StartCoroutine(FadeIn(continueButton, 0.5f));
        yield return StartCoroutine(FadeIn(exitButton, 0.5f));
    }

    IEnumerator FadeIn(CanvasGroup cg, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        cg.alpha = 1f;
    }

    void SetAlpha(CanvasGroup cg, float alpha)
    {
        cg.alpha = alpha;
    }
}