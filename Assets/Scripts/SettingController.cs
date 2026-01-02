using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private GameObject fogFX;

    [SerializeField] private Image fogButtonFX1;
    [SerializeField] private Image fogButtonFX2;


    private bool fogState = true;
    [SerializeField] private bool mainMenu;

    private void Start()
    {
        LoadVolume();

        if(mainMenu)
            return;

        fogState = PlayerPrefs.GetInt("FogState", 1) == 1; // default ON
        ApplyFogState();
    }
    public void SetMusicVolume()
    {
        if (musicSlider == null) return;

        float value = musicSlider.value;
        float dB;

        dB = Mathf.Log10(value) * 20f;

        audioMixer.SetFloat("Music", dB);
        PlayerPrefs.SetFloat("musicVolume", value);
    }


    public void SetSFXVolume()
    {
        if (sfxSlider == null) return;

        float value = sfxSlider.value; // 0..1
        float dB;

        if (value <= 0.0001f)
            dB = -80f;
        else
            dB = Mathf.Log10(value) * 20f;

        audioMixer.SetFloat("Sfx", dB);
        PlayerPrefs.SetFloat("sfxVolume", value);
    }


    private void LoadVolume()
    {
        if (musicSlider == null || sfxSlider == null)
            return;

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", .5f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", .5f);

        SetMusicVolume();
        SetSFXVolume();

    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void FogActive()
    {
        fogState = !fogState;
        ApplyFogState();
        SaveFogState();
    }

    void SaveFogState()
    {
        PlayerPrefs.SetInt("FogState", fogState ? 1 : 0);
        PlayerPrefs.Save();
    }

    void ApplyFogState()
    {

        fogFX.SetActive(fogState);

        Color c = fogState ? Color.green : Color.red;
        fogButtonFX1.color = c;
        fogButtonFX2.color = c;
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Quit Game (Editor không thoát thật)");
#endif
    }
}
