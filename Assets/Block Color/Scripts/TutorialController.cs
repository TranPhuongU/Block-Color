using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public enum ScaleMode
    {
        Both,        // Scale width + height
        HeightOnly   // Chỉ scale height
    }

    [System.Serializable]
    public struct RectExpandConfig
    {
        public RectTransform rect;
        public ScaleMode scaleMode;

        [Header("Target Size")]
        public float targetWidth;
        public float targetHeight;

        public float duration;
    }

    [Header("Rect Configs")]
    [SerializeField] private RectExpandConfig topRect;
    [SerializeField] private RectExpandConfig downRect;

    [Header("Fade Targets")]
    [SerializeField] private Image tutorialImage1;
    [SerializeField] private Image tutorialImage2;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Root")]
    [SerializeField] private GameObject tutorialPanel;

    private Coroutine mainRoutine;

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);

        ResetAlpha();
        ResetRectBaseline(topRect);
        ResetRectBaseline(downRect);

        if (mainRoutine != null)
            StopCoroutine(mainRoutine);

        mainRoutine = StartCoroutine(TutorialSequence());
    }

    public void HideTutorial()
    {
        if (mainRoutine != null)
            StopCoroutine(mainRoutine);

        tutorialPanel.SetActive(false);
    }

    private IEnumerator TutorialSequence()
    {
        yield return StartCoroutine(ExpandRect(topRect));

        Coroutine downRoutine = StartCoroutine(ExpandRect(downRect));

        yield return new WaitForSeconds(downRect.duration * 0.7f);
        yield return StartCoroutine(FadeContent(0f, 1f, 0.4f));

        yield return downRoutine;

    }


    private IEnumerator ExpandRect(RectExpandConfig config)
    {
        if (config.rect == null)
            yield break;

        Vector2 startSize;
        Vector2 targetSize;

        switch (config.scaleMode)
        {
            case ScaleMode.Both:
                startSize = Vector2.zero;
                targetSize = new Vector2(config.targetWidth, config.targetHeight);
                break;

            case ScaleMode.HeightOnly:
                startSize = new Vector2(config.rect.sizeDelta.x, 0f);
                targetSize = new Vector2(config.rect.sizeDelta.x, config.targetHeight);
                break;

            default:
                yield break;
        }

        config.rect.sizeDelta = startSize;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, config.duration);
            float ease = Mathf.SmoothStep(0f, 1f, t);
            config.rect.sizeDelta = Vector2.Lerp(startSize, targetSize, ease);
            yield return null;
        }

        config.rect.sizeDelta = targetSize;
    }

    // ================= FADE =================

    private IEnumerator FadeContent(float from, float to, float fadeDuration)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, fadeDuration);
            float a = Mathf.Lerp(from, to, t);

            SetAlpha(tutorialImage1, a);
            SetAlpha(tutorialImage2, a);
            SetAlpha(tutorialText, a);

            yield return null;
        }

        SetAlpha(tutorialImage1, to);
        SetAlpha(tutorialImage2, to);
        SetAlpha(tutorialText, to);
    }

    // ================= HELPERS =================

    private void ResetRectBaseline(RectExpandConfig config)
    {
        if (config.rect == null) return;

        switch (config.scaleMode)
        {
            case ScaleMode.Both:
                config.rect.sizeDelta = Vector2.zero;
                break;

            case ScaleMode.HeightOnly:
                config.rect.sizeDelta =
                    new Vector2(config.rect.sizeDelta.x, 0f);
                break;
        }
    }

    private void ResetAlpha()
    {
        SetAlpha(tutorialImage1, 0f);
        SetAlpha(tutorialImage2, 0f);
        SetAlpha(tutorialText, 0f);
    }

    private void SetAlpha(Graphic g, float a)
    {
        if (g == null) return;
        Color c = g.color;
        c.a = a;
        g.color = c;
    }
}
