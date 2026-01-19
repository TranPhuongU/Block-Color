using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWindowAnimator : MonoBehaviour
{
    [SerializeField] private float targetScale = 1f;   // scale cuối = 1
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 0.5f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Play()
    {
        StartCoroutine(ScaleAndFadeIn());
    }

    public void Reload()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private IEnumerator ScaleAndFadeIn()
    {
        // trạng thái ban đầu
        rectTransform.localScale = Vector3.one * (targetScale * 0.2f);
        canvasGroup.alpha = 0f;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float ease = Mathf.SmoothStep(0f, 1f, t);

            rectTransform.localScale = Vector3.Lerp(
                Vector3.one * (targetScale * 0.6f),
                Vector3.one * targetScale,
                ease
            );

            canvasGroup.alpha = ease;

            yield return null;
        }

        // snap chuẩn
        rectTransform.localScale = Vector3.one * targetScale;
        canvasGroup.alpha = 1f;
    }
}
