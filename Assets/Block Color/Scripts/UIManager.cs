using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Color[] colors;

    [SerializeField] private TextMeshProUGUI targetColorText;
    [SerializeField] private TextMeshProUGUI moveText;

    [SerializeField] private GameObject createModePanel;

    [SerializeField] private Sprite[] backgroundSprite;
    [SerializeField] private Image backgroundImage;

    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject settingPanel;

    [SerializeField] private TextMeshProUGUI endGameText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        RandomBackground();
    }
    IEnumerator Start()
    {
        GameManager.instance.onGameStateCallBack += EndGamePanelActive;

        yield return null;
        UpdateTargetColorText();
        UpdateMoveText(GameManager.instance.board.currentMoveAmount);

        SetupPanel(GameManager.instance.createModeSO.createMode);
    }
    private void OnDestroy()
    {
        GameManager.instance.onGameStateCallBack -= EndGamePanelActive;

    }

    public void EndGamePanelActive(GameState gameState)
    {
        if (gameState == GameState.Intro || gameState == GameState.Gameplay)
            return;


        endGamePanel.SetActive(true);

        UIWindowAnimator endGameUIController = endGamePanel.GetComponentInChildren<UIWindowAnimator>();

        endGameUIController.Play();

        if (gameState == GameState.Win)
            endGameText.text = "WIN";
        else
            endGameText.text = "LOSE!";
    }
    public void SettingPanelActive()
    {
        gamePanel.SetActive(false);

        settingPanel.SetActive(true);

        UIWindowAnimator endGameUIController = settingPanel.GetComponentInChildren<UIWindowAnimator>();

        endGameUIController.Play();
    }
    public void ReloadButtonPress()
    {
        GameManager.instance.board.ReloadButton();
    }

    private void SetupPanel(bool createMode)
    {
        if (createMode)
        {
            createModePanel.SetActive(true);
        }
        else
        {
            createModePanel.SetActive(false);
        }
    }
    public void UpdateTargetColorText()
    {
        if (targetColorText != null)
        {
            Color color = GetColorFromPiece(GameManager.instance.board.targetColor);
            string hex = UnityEngine.ColorUtility.ToHtmlStringRGB(color);

            string colorName = GameManager.instance.board.targetColor.ToString();
            colorName = char.ToUpper(colorName[0]) + colorName.Substring(1).ToLower();

            targetColorText.text =
                $"Turn all blocks into <b><color=#{hex}>{colorName}</color></b>";
        }
    }

    private void RandomBackground()
    {
        int randomBG = Random.Range(0, backgroundSprite.Length);

        backgroundImage.sprite = backgroundSprite[randomBG];
    }
    public void UpdateMoveText(int moveAmount)
    {
        if (moveText != null)
        {
            moveText.text = moveAmount.ToString();

        }
    }
    Color GetColorFromPiece(PieceColor color)
    {
        switch (color)
        {
            case PieceColor.Blue: return colors[0];
            case PieceColor.Red: return colors[1];
            case PieceColor.Yellow: return colors[2];
            case PieceColor.Green: return colors[3];
            default: return Color.white;
        }
    }
}
