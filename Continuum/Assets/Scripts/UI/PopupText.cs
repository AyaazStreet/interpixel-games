using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    const float FADE = 1f;
    const float MOVE = 80f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < FADE)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / FADE);
            elapsedTime += Time.deltaTime;

            Vector2 pos = rectTransform.anchoredPosition;
            pos.y += MOVE * Time.deltaTime;
            rectTransform.anchoredPosition = pos;

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}
