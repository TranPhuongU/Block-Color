using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource[] musicSource;

    [SerializeField] AudioSource[] winSound;

    [SerializeField] AudioSource[] loseSound;

    [SerializeField] private AudioClip clearPieceClip;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioSource buttonPressSound;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        PlayRandomMusic();
    }

    public void PlayRandomSound(AudioSource[] audioSource)
    {
        if (audioSource.Length == 0) return;
        int index = Random.Range(0, audioSource.Length);
        audioSource[index].Play();
    }

    public void PlayRandomMusic()
    {
        PlayRandomSound(musicSource);
    }


    public void PlayWinSound()
    {
        StopMusic();
        PlayRandomSound(winSound);
    }

    public void PlayLoseSound()
    {
        StopMusic();
        PlayRandomSound(loseSound);
    }

    void StopMusic()
    {
        foreach (var src in musicSource)
        {
            if (src.isPlaying)
                src.Pause(); // hoặc Stop()
        }
    }

    public void PlayClearPieceSound(int waveIndex)
    {
        if (clearPieceClip == null || sfxSource == null) return;

        // 🎵 tăng pitch theo lượt
        float pitch = 1f + waveIndex * 0.05f;
        pitch = Mathf.Clamp(pitch, 1f, 3f); // tránh quá chói

        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clearPieceClip, 0.5f);
    }

    
    public void PlayButtonSound()
    {
        buttonPressSound.Play();
    }

}
