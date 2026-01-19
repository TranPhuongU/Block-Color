using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{

    

    [SerializeField] private GameObject fogFX;

    [SerializeField] private Image fogButtonFX1;
    [SerializeField] private Image fogButtonFX2;


    private bool fogState = true;
    [SerializeField] private bool mainMenu;

    private void Start()
    {
        if(mainMenu)
            return;

        fogState = PlayerPrefs.GetInt("FogState", 1) == 1; // default ON
        ApplyFogState();
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
