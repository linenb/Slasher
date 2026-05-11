using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimalDeathAnimation : MonoBehaviour
{
    public Sprite[] deathFrames;  
    public float fps = 12f;

    private SpriteRenderer sr;
    private bool isPlaying = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void PlayDeath()
    {
    if (FoxAnimation.instance != null)
            FoxAnimation.instance.StopIdle();
    
    StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        isPlaying = true;

        float delay = 1f / fps;

        for (int i = 0; i < deathFrames.Length; i++)
        {
            sr.sprite = deathFrames[i];
            yield return new WaitForSeconds(delay);
        }

        // After animation → game over
        if (GameManager.instance != null)
        {
            GameManager.instance.TriggerGameOver();
        }
    }
}