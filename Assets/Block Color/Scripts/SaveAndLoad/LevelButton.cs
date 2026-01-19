using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelButton : MonoBehaviour
{
    public Text levelNumberText;

    private Button button;
    private Image bg;

    public Sprite unlockedSprite;
    public Sprite lockedSprite;


    void Awake()
    {
        button = GetComponent<Button>();
        bg = GetComponent<Image>();
    }

    public void UpdateView(LevelProgress data)
    {
        bool unlocked = data.unlocked;

        bg.sprite = unlocked ? unlockedSprite : lockedSprite;
        button.interactable = unlocked;

    }

    public void SetClickAction(System.Action callback)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback());
    }

    public void SetLevelNumber(int number)
    {
        if (levelNumberText != null)
            levelNumberText.text = number.ToString();
    }

}
