using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FXManager : MonoBehaviour
{
    public static FXManager instance;

    [SerializeField] private ParticleSystem[] winParticles;
    [SerializeField] private GameObject fogFx;

    [SerializeField] private float fogFadeDuration = 0.8f;

    private void Awake()
    {
        instance = this;
    }

    public void PlayRandomWinEffect()
    {

        FadeFog();

        if (winParticles == null || winParticles.Length == 0)
            return;

        int randomIndex = Random.Range(0, winParticles.Length);
        winParticles[randomIndex].Play();
    }

    public void FadeFog()
    {
        if (fogFx == null)
            return;

        StartCoroutine(FadeFogRoutine());
    }

    IEnumerator FadeFogRoutine()
    {
        Image[] images = fogFx.GetComponentsInChildren<Image>();

        if (images.Length == 0)
            yield break;

        float time = 0f;

        // Lưu alpha ban đầu (phòng trường hợp không phải 1)
        float[] startAlphas = new float[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            startAlphas[i] = images[i].color.a;
        }

        while (time < fogFadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fogFadeDuration;

            for (int i = 0; i < images.Length; i++)
            {
                Color c = images[i].color;
                c.a = Mathf.Lerp(startAlphas[i], 0f, t);
                images[i].color = c;
            }

            yield return null;
        }

        // đảm bảo alpha = 0
        for (int i = 0; i < images.Length; i++)
        {
            Color c = images[i].color;
            c.a = 0f;
            images[i].color = c;
        }

        fogFx.SetActive(false);
    }
}
