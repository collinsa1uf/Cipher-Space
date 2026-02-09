using System;
using System.Collections;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] public CanvasGroup fadeInCanvasGroup;
    [SerializeField] public CanvasGroup fadeOutCanvasGroup;
    public float fadeDuration = 1.0f;
   
    public void Fade()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Fades out currently oqaque canvas group
        yield return StartCoroutine(FadeRoutine(fadeOutCanvasGroup, fadeOutCanvasGroup.alpha, 0f, fadeDuration));
        // Ensure previous coroutine finished
        yield return new WaitForSeconds(fadeDuration);
        // Sets fade out canvas group objects to be not interactable
        fadeOutCanvasGroup.interactable = false;
        // Sets fade in canvas group object to be interactable
        fadeInCanvasGroup.interactable = true;
        // Fades in new, previously transparent canvas group
        yield return StartCoroutine(FadeRoutine(fadeInCanvasGroup, fadeInCanvasGroup.alpha, 1f, fadeDuration));
        // Sets fade out canvas group object to be inactive
        fadeOutCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator FadeRoutine(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure float value fully reaches end alpha
        if (endAlpha == 0f)
        {
            cg.alpha = 0f;
        }
        else if (endAlpha == 1f)
        {
            cg.alpha = 1f;
        }
    }
}
