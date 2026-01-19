using System.Collections;
using TMPro;
using UnityEngine;

public class TextFX : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    public float fadeDuration = 0.5f; // thời gian fade

    private void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        // Set alpha ban đầu = 0
        Color c = textMeshProUGUI.color;
        c.a = 0f;
        textMeshProUGUI.color = c;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            Color c = textMeshProUGUI.color;
            c.a = Mathf.Lerp(0f, 1f, t);
            textMeshProUGUI.color = c;

            yield return null;
        }

        // đảm bảo alpha = 1
        Color finalColor = textMeshProUGUI.color;
        finalColor.a = 1f;
        textMeshProUGUI.color = finalColor;
    }
}
